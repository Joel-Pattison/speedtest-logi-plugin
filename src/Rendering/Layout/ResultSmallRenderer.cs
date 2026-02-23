namespace Loupedeck.SpeedTestPlugin.Rendering.Layout
{
    using System;

    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;

    public class ResultSmallRenderer : IStateRenderer
    {
        public Boolean CanRender(SpeedTestState state, DisplayFormat format) => state.IsDone && format == DisplayFormat.Small;

        public void Render(ImageBuilder builder, SpeedTestState state)
        {
            var parts = state.GetSpeedParts();
            DrawSmallResults(builder, SpeedTestTheme.Dimensions.ReferenceResolution, SpeedTestTheme.Dimensions.ReferenceResolution, parts[1], parts[2]);
        }

        private static void DrawSmallResults(ImageBuilder builder, Int32 width, Int32 height, String download, String upload)
        {
            var downloadText = SpeedTestTheme.Icons.Download + download;
            var uploadText = SpeedTestTheme.Icons.Upload + upload;

            var downloadY = (Int32)(height * 0.25);
            var uploadY = (Int32)(height * 0.55);

            builder.DrawHorizontallyCenteredText(downloadText, 25, SpeedTestTheme.Colors.Download, downloadY, width);
            builder.DrawHorizontallyCenteredText(uploadText, 25, SpeedTestTheme.Colors.Upload, uploadY, width);
        }
    }
}
