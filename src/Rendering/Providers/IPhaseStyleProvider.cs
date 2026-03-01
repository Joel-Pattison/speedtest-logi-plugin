namespace Loupedeck.SpeedTestPlusPlugin.Rendering.Providers
{
    using Loupedeck.SpeedTestPlusPlugin.Models;

    public interface IPhaseStyleProvider
    {
        PhaseStyle GetPhaseStyle(SpeedTestState state);
    }
}
