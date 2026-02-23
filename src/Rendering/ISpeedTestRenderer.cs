namespace Loupedeck.SpeedTestPlugin.Rendering
{
    using Loupedeck.SpeedTestPlugin.Models;

    public interface ISpeedTestRenderer
    {
        BitmapImage Render(SpeedTestState state, PluginImageSize imageSize);
    }
}
