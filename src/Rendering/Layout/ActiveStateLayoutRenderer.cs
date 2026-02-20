namespace Loupedeck.SpeedTestPlugin.Rendering.Layout
{
    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;
    using Loupedeck.SpeedTestPlugin.Rendering.Providers;

    using SkiaSharp;

    public class ActiveStateRenderer(IPhaseStyleProvider phaseStyleProvider) : IStateRenderer
    {
        private readonly IPhaseStyleProvider _phaseStyleProvider = phaseStyleProvider;

        public Boolean CanRender(SpeedTestState state) => state.IsActive;

        public void Render(ImageBuilder builder, Int32 width, Int32 height, SpeedTestState state)
        {
            var phaseStyle = this._phaseStyleProvider.GetPhaseStyle(state);
            DrawActiveStateWithValue(builder, width, height, state, phaseStyle, state.Speed);
            DrawProgressBar(builder, width, height, state.Progress, phaseStyle.Color);
        }

        private static void DrawActiveStateWithValue(ImageBuilder builder, Int32 width, Int32 height,
            SpeedTestState state, PhaseStyle style, String valueStr)
        {
            var headerText = state.Phase.ToString().ToUpper();
            var unitText = $"{style.Icon} {style.Unit}";

            var headerY = (Int32)(height * 0.17);
            var valueY = (Int32)(height * 0.34);
            var unitY = (Int32)(height * 0.70);

            builder.DrawHorizontallyCenteredText(headerText, SpeedTestTheme.Fonts.Small, SpeedTestTheme.Colors.Gray, headerY, width);
            builder.DrawHorizontallyCenteredText(valueStr, SpeedTestTheme.Fonts.Large, style.Color, valueY, width);
            builder.DrawHorizontallyCenteredText(unitText, SpeedTestTheme.Fonts.Small, SpeedTestTheme.Colors.Gray, unitY, width);
        }

        private static void DrawProgressBar(ImageBuilder builder, Int32 width, Int32 height, String progress, SKColor color)
        {
            if (Double.TryParse(progress, out var pVal))
            {
                var barWidth = (Int32)(width * pVal);
                builder.FillRectangle(0, height - SpeedTestTheme.Dimensions.ProgressBarHeight, width, SpeedTestTheme.Dimensions.ProgressBarHeight, SpeedTestTheme.Colors.BarBg);
                builder.FillRectangle(0, height - SpeedTestTheme.Dimensions.ProgressBarHeight, barWidth, SpeedTestTheme.Dimensions.ProgressBarHeight, color);
            }
        }
    }
}