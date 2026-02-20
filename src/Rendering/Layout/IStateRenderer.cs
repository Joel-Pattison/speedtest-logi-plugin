namespace Loupedeck.SpeedTestPlugin.Rendering.Layout
{
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;

    public interface IStateRenderer
    {
        Boolean CanRender(SpeedTestState state);
        void Render(ImageBuilder builder, Int32 width, Int32 height, SpeedTestState state);
    }
}