namespace Loupedeck.SpeedTestPlugin.Constants
{
    using System;

    using SkiaSharp;

    public static class SpeedTestTheme
    {
        public static class Colors
        {
            public static readonly SKColor Ping = new(255, 170, 0);
            public static readonly SKColor Download = new(0, 255, 150);
            public static readonly SKColor Upload = new(50, 150, 255);
            public static readonly SKColor Text = new(255, 255, 255);
            public static readonly SKColor Gray = new(140, 140, 140);
            public static readonly SKColor Error = new(255, 80, 80);
            public static readonly SKColor BarBg = new(40, 40, 40);
            public static readonly SKColor Black = SKColors.Black;
        }

        public static class Dimensions
        {
            /// <summary>The reference resolution that all fonts and gaps are designed for (116x116).</summary>
            public const Int32 ReferenceResolution = 116;

            public const Int32 IconPadding = 20;
            public const Int32 ProgressBarHeight = 6;

            public const Int32 GapMicro = 2;
            public const Int32 GapTiny = 4;
            public const Int32 GapSmall = 8;
            public const Int32 GapMedium = 14;
            public const Int32 GapLarge = 20;
        }

        public static class Fonts
        {
            public const Int32 Large = 32;
            public const Int32 Medium = 22;
            public const Int32 Small = 14;
            public const Int32 Tiny = 13;
            public const Int32 Header = 11;
            public const Int32 Error = 10;
        }

        public static class Icons
        {
            public const String Ping = "↕";
            public const String Download = "↓";
            public const String Upload = "↑";
        }

        public static class Units
        {
            public const String Mbps = "Mbps";
            public const String Ms = "ms";
        }
    }
}
