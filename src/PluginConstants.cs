namespace Loupedeck.SpeedTestPlugin
{
    using System;

    /// <summary>Centralised string constants used across the plugin.</summary>
    public static class PluginConstants
    {
        // Executable
        public static readonly String ExeName = Loupedeck.Helpers.IsWindows() ? "api.exe" : "api";

        public static readonly String ExeResourceName = Loupedeck.Helpers.IsWindows()
            ? "Loupedeck.SpeedTestPlugin.External.api.exe"
            : "Loupedeck.SpeedTestPlugin.External.api";
    }
}
