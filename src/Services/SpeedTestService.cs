namespace Loupedeck.SpeedTestPlugin.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;

    public class SpeedTestService : ISpeedTestService
    {
        private const Double BandwidthToBytesPerSecond = 125000.0;
        private const Double MinimumProgress = 0.01;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly Dictionary<String, SpeedTestEventType> EventTypeMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["ping"] = SpeedTestEventType.Ping,
            ["download"] = SpeedTestEventType.Download,
            ["upload"] = SpeedTestEventType.Upload,
            ["result"] = SpeedTestEventType.Result
        };

        private CancellationTokenSource _currentTestCancellation;
        private readonly Object _cancellationLock = new();

        public async Task RunTest(IProgress<SpeedTestState> progress, String pluginDataDirectory)
        {
            CancellationTokenSource cancellationSource;

            lock (this._cancellationLock)
            {
                this._currentTestCancellation?.Cancel();
                this._currentTestCancellation?.Dispose();
                this._currentTestCancellation = new CancellationTokenSource();
                cancellationSource = this._currentTestCancellation;
            }

            var state = new SpeedTestState();

            try
            {
                var exePath = Path.Combine(pluginDataDirectory, PluginConstants.ExeName);

                if (!File.Exists(exePath))
                {
                    PluginLog.Error($"SpeedTest executable not found at {exePath}. Ensure plugin is installed correctly.");
                    UpdateState(state, progress, SpeedTestPhase.Error, "No Exe", "");
                    return;
                }

                UpdateState(state, progress, SpeedTestPhase.Ping, "0", MinimumProgress.ToString());

                await this.ExecuteSpeedTestProcess(exePath, state, progress, cancellationSource.Token);
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Error running Speed Test.");
                UpdateState(state, progress, SpeedTestPhase.Error, "Exception", "");
            }
        }

        private static void UpdateState(SpeedTestState state, IProgress<SpeedTestState> progress, SpeedTestPhase phase, String speed, String prog)
        {
            state.Phase = phase;
            state.Speed = speed;
            state.Progress = prog;
            progress.Report(state);
        }

        private Task ExecuteSpeedTestProcess(String exePath, SpeedTestState state, IProgress<SpeedTestState> progress, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Process process = null;
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        Arguments = PluginConstants.ExeArguments,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    process = Process.Start(startInfo);
                    PluginLog.Info($"Speed test process started (PID: {process?.Id}).");

                    using (cancellationToken.Register(() =>
                    {
                        try
                        {
                            if (process != null && !process.HasExited)
                            {
                                process.Kill();
                                PluginLog.Info("Speed test process killed due to cancellation.");
                            }
                        }
                        catch (Exception ex)
                        {
                            PluginLog.Warning(ex, "Failed to kill speed test process.");
                        }
                    }))
                    {
                        String line;
                        while ((line = process.StandardOutput.ReadLine()) != null)
                        {
                            if (String.IsNullOrWhiteSpace(line))
                            {
                                continue;
                            }

                            try
                            {
                                this.ProcessJsonLine(line, state, progress);
                            }
                            catch (Exception innerEx)
                            {
                                PluginLog.Warning(innerEx, $"Failed to parse JSON line: {line}");
                            }
                        }

                        process.WaitForExit();
                        PluginLog.Info($"Speed test process exited with code {process.ExitCode}.");
                    }
                }
                catch (Exception ex)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        PluginLog.Error(ex, "Error executing SpeedTest process.");
                        UpdateState(state, progress, SpeedTestPhase.Error, "Process", "");
                    }
                }
                finally
                {
                    process?.Dispose();
                }
            }, cancellationToken);
        }

        private void ProcessJsonLine(String line, SpeedTestState state, IProgress<SpeedTestState> progress)
        {
            try
            {
                var eventData = JsonSerializer.Deserialize<SpeedTestEvent>(line, JsonOptions);
                if (eventData == null || String.IsNullOrEmpty(eventData.Type))
                {
                    return;
                }

                var eventType = ParseEventType(eventData.Type);
                this.HandleEvent(eventType, eventData, state, progress);
            }
            catch (JsonException ex)
            {
                PluginLog.Warning(ex, "Failed to deserialize speed test event");
            }
        }

        private static SpeedTestEventType ParseEventType(String type)
        {
            return EventTypeMap.TryGetValue(type, out var eventType)
                ? eventType
                : SpeedTestEventType.Unknown;
        }

        private void HandleEvent(SpeedTestEventType eventType, SpeedTestEvent eventData, SpeedTestState state, IProgress<SpeedTestState> progress)
        {
            switch (eventType)
            {
                case SpeedTestEventType.Ping:
                    HandlePingEvent(eventData.Ping, state, progress);
                    break;

                case SpeedTestEventType.Download:
                    HandleBandwidthEvent(SpeedTestPhase.Download, eventData.Download, state, progress);
                    break;

                case SpeedTestEventType.Upload:
                    HandleBandwidthEvent(SpeedTestPhase.Upload, eventData.Upload, state, progress);
                    break;

                case SpeedTestEventType.Result:
                    HandleResultEvent(eventData, state, progress);
                    break;

                default:
                    PluginLog.Verbose($"Ignoring unknown event type: {eventData.Type}");
                    break;
            }
        }

        private static void HandlePingEvent(PingData pingData, SpeedTestState state, IProgress<SpeedTestState> progress)
        {
            if (pingData == null)
            {
                return;
            }

            var latency = pingData.Latency.ToString("F0");
            var progressValue = pingData.Progress.ToString();

            UpdateState(state, progress, SpeedTestPhase.Ping, latency, progressValue);
        }

        private static void HandleBandwidthEvent(SpeedTestPhase phase, BandwidthData bandwidthData, SpeedTestState state, IProgress<SpeedTestState> progress)
        {
            if (bandwidthData == null)
            {
                return;
            }

            var mbps = ConvertBandwidthToMbps(bandwidthData.Bandwidth);
            var progressValue = bandwidthData.Progress.ToString();

            UpdateState(state, progress, phase, mbps, progressValue);
        }

        private static void HandleResultEvent(SpeedTestEvent eventData, SpeedTestState state, IProgress<SpeedTestState> progress)
        {
            if (eventData.Download == null || eventData.Upload == null || eventData.Ping == null)
            {
                return;
            }

            var downloadMbps = ConvertBandwidthToMbps(eventData.Download.Bandwidth);
            var uploadMbps = ConvertBandwidthToMbps(eventData.Upload.Bandwidth);
            var pingMs = eventData.Ping.Latency.ToString("F0");

            if (eventData.Server != null && !String.IsNullOrEmpty(eventData.Server.Location))
            {
                state.ServerLocation = eventData.Server.Location;
                PluginLog.Info($"Speed test server: {eventData.Server.Location}");
            }

            PluginLog.Info($"Speed Test Result: {downloadMbps} Mbps Down, {uploadMbps} Mbps Up, {pingMs} ms Ping");
            UpdateState(state, progress, SpeedTestPhase.Done, $"{pingMs}/{downloadMbps}/{uploadMbps}", "1.0");
        }

        private static String ConvertBandwidthToMbps(Double bandwidth, String format = "F0")
        {
            var mbps = bandwidth / BandwidthToBytesPerSecond;
            return mbps.ToString(format);
        }
    }
}