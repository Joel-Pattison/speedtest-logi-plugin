namespace Loupedeck.SpeedTestPlugin.Rendering.Layout
{
    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;

    using SkiaSharp;

    public class ResultStateRenderer : IStateRenderer
    {
        public Boolean CanRender(SpeedTestState state) => state.IsDone;

        public void Render(ImageBuilder builder, Int32 width, Int32 height, SpeedTestState state)
        {
            var parts = state.GetSpeedParts();
            this.DrawResults(builder, width, height, state.ServerLocation, parts[0], parts[1], parts[2]);
        }

        private void DrawResults(ImageBuilder builder, Int32 width, Int32 height, String serverLocation, String ping, String download, String upload)
        {
            var downloadTextHeight = ImageBuilder.MeasureTextHeight(SpeedTestTheme.Fonts.Medium, SpeedTestTheme.Icons.Download + " " + download);
            var uploadTextHeight = ImageBuilder.MeasureTextHeight(SpeedTestTheme.Fonts.Medium, SpeedTestTheme.Icons.Upload + " " + upload);
            var pingTextHeight = ImageBuilder.MeasureTextHeight(SpeedTestTheme.Fonts.Tiny, $"{ping} {SpeedTestTheme.Units.Ms}");
            var locationTextHeight = ImageBuilder.MeasureTextHeight(SpeedTestTheme.Fonts.Tiny, serverLocation);

            var totalContentHeight = downloadTextHeight + SpeedTestTheme.Dimensions.GapSmall + uploadTextHeight + SpeedTestTheme.Dimensions.GapMedium + pingTextHeight + SpeedTestTheme.Dimensions.GapTiny + locationTextHeight;

            var topPadding = (height - totalContentHeight) / 2;
            var currentY = topPadding + 8;

            this.DrawValueWithUnit(builder, SpeedTestTheme.Icons.Download + " " + download, SpeedTestTheme.Units.Mbps, currentY, SpeedTestTheme.Fonts.Medium, SpeedTestTheme.Fonts.Small, SpeedTestTheme.Colors.Download, width);
            currentY += downloadTextHeight + SpeedTestTheme.Dimensions.GapSmall;

            this.DrawValueWithUnit(builder, SpeedTestTheme.Icons.Upload + " " + upload, SpeedTestTheme.Units.Mbps, currentY, SpeedTestTheme.Fonts.Medium, SpeedTestTheme.Fonts.Small, SpeedTestTheme.Colors.Upload, width);
            currentY += uploadTextHeight + SpeedTestTheme.Dimensions.GapMedium;

            builder.DrawHorizontallyCenteredText($"{ping} {SpeedTestTheme.Units.Ms}", SpeedTestTheme.Fonts.Tiny, SpeedTestTheme.Colors.Ping, currentY, width);
            currentY += pingTextHeight + SpeedTestTheme.Dimensions.GapTiny;

            builder.DrawHorizontallyCenteredText(serverLocation, SpeedTestTheme.Fonts.Tiny, SpeedTestTheme.Colors.Gray, currentY, width);
        }

        private void DrawValueWithUnit(ImageBuilder builder, String valueText, String unitText, Int32 y, Int32 valueFontSize, Int32 unitFontSize, SKColor color, Int32 width)
        {
            var valueTextWidth = ImageBuilder.MeasureTextWidth(valueText, valueFontSize);
            var unitTextWidth = ImageBuilder.MeasureTextWidth(unitText, unitFontSize);
            var totalTextWidth = valueTextWidth + SpeedTestTheme.Dimensions.GapTiny + unitTextWidth;
            var startX = (width - totalTextWidth) / 2;

            builder.DrawText(valueText, startX, y, color, valueFontSize);

            var valueBaselineOffset = ImageBuilder.GetBaselineFromTop(valueFontSize, 0);
            var unitBaselineOffset = ImageBuilder.GetBaselineFromTop(unitFontSize, 0);
            var unitY = y + (valueBaselineOffset - unitBaselineOffset);

            builder.DrawText(unitText, startX + valueTextWidth + SpeedTestTheme.Dimensions.GapTiny, unitY, color, unitFontSize);
        }
    }
}