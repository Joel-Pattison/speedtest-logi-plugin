namespace Loupedeck.SpeedTestPlugin.Helpers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using Loupedeck.SpeedTestPlugin.Constants;

    /// <summary>
    /// Manages extraction and cleanup of embedded plugin resources.
    /// </summary>
    internal static class PluginInstaller
    {
        /// <summary>
        /// Ensures that the required executable exists in the plugin data directory.
        /// Extracts from embedded resources if missing.
        /// </summary>
        public static Boolean EnsureInstalled(String pluginDataDirectory, Assembly assembly)
        {
            try
            {
                if (!Directory.Exists(pluginDataDirectory))
                {
                    Directory.CreateDirectory(pluginDataDirectory);
                }

                var exePath = Path.Combine(pluginDataDirectory, PluginConstants.ExeName);

                if (File.Exists(exePath))
                {
                    PluginLog.Info($"PluginInstaller: {PluginConstants.ExeName} already present at {exePath}");
                    return true;
                }

                PluginLog.Info($"PluginInstaller: Extracting {PluginConstants.ExeName} to {exePath}");
                return ExtractResource(assembly, exePath);
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "PluginInstaller: Failed to ensure installation.");
                return false;
            }
        }

        /// <summary>
        /// Removes the plugin data directory and all its contents.
        /// </summary>
        public static Boolean Uninstall(String pluginDataDirectory)
        {
            try
            {
                if (Directory.Exists(pluginDataDirectory))
                {
                    Directory.Delete(pluginDataDirectory, true);
                    PluginLog.Info($"PluginInstaller: Removed directory {pluginDataDirectory}");
                }

                return true;
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "PluginInstaller: Failed to uninstall.");
                return false;
            }
        }

        private static Boolean ExtractResource(Assembly assembly, String exePath)
        {
            using (var stream = assembly.GetManifestResourceStream(PluginConstants.ExeResourceName))
            {
                if (stream == null)
                {
                    PluginLog.Error($"PluginInstaller: Embedded resource '{PluginConstants.ExeResourceName}' not found.");
                    return false;
                }

                using (var fileStream = new FileStream(exePath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }

            if (!Loupedeck.Helpers.IsWindows())
            {
                Process.Start("chmod", $"+x \"{exePath}\"")?.WaitForExit();
            }

            PluginLog.Info($"PluginInstaller: Successfully extracted {PluginConstants.ExeName}");
            return true;
        }
    }
}