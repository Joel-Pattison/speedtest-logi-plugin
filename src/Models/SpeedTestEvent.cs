namespace Loupedeck.SpeedTestPlugin.Models
{
    using System;
    using System.Text.Json.Serialization;

    internal enum SpeedTestEventType
    {
        Unknown,
        Ping,
        Download,
        Upload,
        Result
    }

    internal class SpeedTestEvent
    {
        [JsonPropertyName("type")]
        public String Type { get; set; }

        [JsonPropertyName("ping")]
        public PingData Ping { get; set; }

        [JsonPropertyName("download")]
        public BandwidthData Download { get; set; }

        [JsonPropertyName("upload")]
        public BandwidthData Upload { get; set; }

        [JsonPropertyName("server")]
        public ServerData Server { get; set; }
    }

    internal class PingData
    {
        [JsonPropertyName("latency")]
        public Double Latency { get; set; }

        [JsonPropertyName("progress")]
        public Double Progress { get; set; }
    }

    internal class BandwidthData
    {
        [JsonPropertyName("bandwidth")]
        public Double Bandwidth { get; set; }

        [JsonPropertyName("progress")]
        public Double Progress { get; set; }
    }

    internal class ServerData
    {
        [JsonPropertyName("location")]
        public String Location { get; set; }
    }
}
