namespace Loupedeck.SpeedTestPlugin.Rendering.Providers
{
    using Loupedeck.SpeedTestPlugin.Models;

    public interface IPhaseStyleProvider
    {
        PhaseStyle GetPhaseStyle(SpeedTestState state);
    }
}