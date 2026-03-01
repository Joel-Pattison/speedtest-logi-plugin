namespace Loupedeck.SpeedTestPlusPlugin.Rendering.Layout
{
    using Loupedeck.SpeedTestPlusPlugin.Helpers;
    using Loupedeck.SpeedTestPlusPlugin.Models;

    public interface IStateRenderer
    {
        Boolean CanRender(SpeedTestState state, DisplayFormat format);
        void Render(ImageBuilder builder, SpeedTestState state);
    }
}
