namespace Loupedeck.SpeedTestPlugin.Models
{
    using SkiaSharp;

    public readonly struct PhaseStyle
    {
        public PhaseStyle(SKColor color, String icon, String unit) =>
            (this.Color, this.Icon, this.Unit) = (color, icon, unit);

        public SKColor Color { get; }
        public String Icon { get; }
        public String Unit { get; }
    }
}