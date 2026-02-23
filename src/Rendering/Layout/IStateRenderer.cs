namespace Loupedeck.SpeedTestPlugin.Rendering.Layout
{
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;

    public interface IStateRenderer
    {
        Boolean CanRender(SpeedTestState state, DisplayFormat format);
        void Render(ImageBuilder builder, SpeedTestState state);
    }
}
