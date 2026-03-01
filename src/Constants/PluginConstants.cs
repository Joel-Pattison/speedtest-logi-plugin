namespace Loupedeck.SpeedTestPlusPlugin.Constants
{
    using System;

    /// <summary>Centralised string constants used across the plugin.</summary>
    public static class PluginConstants
    {
        // Command metadata
        public const String CommandDisplayName = "Run Speed Test";
        public const String CommandDescription = "Run a speed test";

        // Font
        public const String FontFileName = "Inter_18pt-Medium.ttf";

        // Executable
        public static readonly String ExeName = Loupedeck.Helpers.IsWindows() ? "api.exe" : "api";

        public static readonly String ExeResourceName = Loupedeck.Helpers.IsWindows()
            ? "Loupedeck.SpeedTestPlusPlugin.External.api.exe"
            : "Loupedeck.SpeedTestPlusPlugin.External.api";

        public const String ImageResourceName = "Loupedeck.SpeedTestPlusPlugin.package.metadata.Icon256x256.png";

        public const String ExeArguments = "--accept-license --accept-gdpr --progress=yes -f jsonl";
    }
}
