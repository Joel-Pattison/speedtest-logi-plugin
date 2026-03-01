namespace Loupedeck.SpeedTestPlusPlugin.Rendering
{
    using Loupedeck.SpeedTestPlusPlugin.Models;

    public interface ISpeedTestRenderer
    {
        BitmapImage Render(SpeedTestState state, PluginImageSize imageSize);
    }
}
