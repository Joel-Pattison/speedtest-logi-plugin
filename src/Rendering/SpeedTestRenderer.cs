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
                new ResultStateRenderer(),
                new ReadyStateRenderer(),
                new ActiveStateRenderer(phaseStyleProvider)
            };
        }

        public BitmapImage Render(SpeedTestState state, PluginImageSize imageSize)
        {
            var width = imageSize.GetButtonWidth();
            var height = imageSize.GetButtonHeight();

            try
            {
                using (var builder = new ImageBuilder(width, height))
                {
                    builder.Clear(SpeedTestTheme.Colors.Black);
                    this.RenderState(builder, width, height, state);
                    return builder.ToBitmapImage();
                }
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Error in SpeedTestRenderer");
                return RenderError(width, height, ex.Message);
            }
        }

        private void RenderState(ImageBuilder builder, Int32 width, Int32 height, SpeedTestState state)
        {
            var renderer = this._renderers.FirstOrDefault(r => r.CanRender(state));
            if (renderer == null)
            {
                PluginLog.Warning($"No renderer found for state: {state.Phase}");
                return;
            }

            renderer.Render(builder, width, height, state);
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