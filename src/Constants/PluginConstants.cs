namespace Loupedeck.SpeedTestPlugin.Constants
{
    using System;

    /// <summary>Centralised string constants used across the plugin.</summary>
    public static class PluginConstants
    {
        // Font
        public const String FontFileName = "Inter_18pt-Medium.ttf";

        // Executable
        public static readonly String ExeName = Loupedeck.Helpers.IsWindows() ? "api.exe" : "api";

        public static readonly String ExeResourceName = Loupedeck.Helpers.IsWindows()
            ? "Loupedeck.SpeedTestPlugin.External.api.exe"
            : "Loupedeck.SpeedTestPlugin.External.api";

        public const String ImageResourceName = "Loupedeck.SpeedTestPlugin.package.metadata.Icon256x256.png";

        public const String ExeArguments = "--accept-license --accept-gdpr --progress=yes -f jsonl";
    }
}
