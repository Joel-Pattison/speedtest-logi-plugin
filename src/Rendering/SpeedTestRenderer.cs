namespace Loupedeck.SpeedTestPlugin.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;
    using Loupedeck.SpeedTestPlugin.Rendering.Layout;
    using Loupedeck.SpeedTestPlugin.Rendering.Providers;

    using SkiaSharp;

    public class SpeedTestRenderer : ISpeedTestRenderer
    {
        private readonly IEnumerable<IStateRenderer> _renderers;

        public SpeedTestRenderer()
        {
            var phaseStyleProvider = new PhaseStyleProvider();
            this._renderers = new IStateRenderer[]
            {
                new ResultNormalRenderer(),
                new ResultSmallRenderer(),
                new ReadyStateRenderer(),
                new ActiveStateNormalRenderer(phaseStyleProvider),
                new ActiveStateSmallRenderer(phaseStyleProvider)
            };
        }

        public BitmapImage Render(SpeedTestState state, PluginImageSize imageSize)
        {
            var targetWidth = imageSize.GetButtonWidth();
            var targetHeight = imageSize.GetButtonHeight();
            PluginLog.Info($"Rendering button with width: {targetWidth}, height: {targetHeight}");

            // Only actions ring should display the small format. We can identify it because actions ring is variable and will not map to
            // the standard enum values.
            var format = Enum.IsDefined(typeof(PluginImageSize), imageSize) ? DisplayFormat.Normal : DisplayFormat.Small;

            try
            {
                using (var builder = new ImageBuilder(targetWidth, targetHeight))
                {
                    builder.SetResolutionScale(SpeedTestTheme.Dimensions.ReferenceResolution, SpeedTestTheme.Dimensions.ReferenceResolution);

                    builder.Clear(SpeedTestTheme.Colors.Black);
                    this.RenderState(builder, state, format);

                    return builder.ToBitmapImage();
                }
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Error in SpeedTestRenderer");
                return RenderError(targetWidth, targetHeight, ex.Message);
            }
        }

        private void RenderState(ImageBuilder builder, SpeedTestState state, DisplayFormat format)
        {
            var renderer = this._renderers.FirstOrDefault(r => r.CanRender(state, format));
            if (renderer == null)
            {
                PluginLog.Warning($"No renderer found for state: {state.Phase}");
                return;
            }

            renderer.Render(builder, state);
        }

        private static BitmapImage RenderError(Int32 width, Int32 height, String message)
        {
            using (var builder = new ImageBuilder(width, height))
            {
                builder.Clear(new SKColor(255, 0, 0)); // Red
                var errorText = $"Error: {message}";
                builder.DrawText(errorText, 2, 2, SpeedTestTheme.Colors.Text, SpeedTestTheme.Fonts.Error);
                return builder.ToBitmapImage();
            }
        }
    }
}
