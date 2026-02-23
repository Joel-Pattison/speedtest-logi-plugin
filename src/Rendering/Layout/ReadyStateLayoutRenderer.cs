namespace Loupedeck.SpeedTestPlugin.Rendering.Layout
{
    using System;

    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;

    public class ReadyStateRenderer : IStateRenderer
    {
        public Boolean CanRender(SpeedTestState state, DisplayFormat format) => state.IsReady;

        public void Render(ImageBuilder builder, SpeedTestState state)
        {
            try
            {
                var iconBytes = EmbeddedResources.ReadBinaryFile(PluginConstants.ImageResourceName);
                if (iconBytes != null)
                {
                    builder.DrawCenteredImage(iconBytes, SpeedTestTheme.Dimensions.ReferenceResolution, SpeedTestTheme.Dimensions.ReferenceResolution, SpeedTestTheme.Dimensions.IconPadding);
                    return;
                }
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, $"Failed to load ready icon: {PluginConstants.ImageResourceName}");
            }
        }
    }
}
