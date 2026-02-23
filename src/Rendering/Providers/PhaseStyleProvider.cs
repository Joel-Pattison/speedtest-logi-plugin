namespace Loupedeck.SpeedTestPlugin.Rendering.Providers
{
    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Models;

    public class PhaseStyleProvider : IPhaseStyleProvider
    {
        public PhaseStyle GetPhaseStyle(SpeedTestState state) =>
            state.Phase switch
            {
                SpeedTestPhase.Error => new PhaseStyle(SpeedTestTheme.Colors.Error, "", ""),
                SpeedTestPhase.Ping => new PhaseStyle(SpeedTestTheme.Colors.Ping, SpeedTestTheme.Icons.Ping, SpeedTestTheme.Units.Ms),
                SpeedTestPhase.Download => new PhaseStyle(SpeedTestTheme.Colors.Download, SpeedTestTheme.Icons.Download, SpeedTestTheme.Units.Mbps),
                SpeedTestPhase.Upload => new PhaseStyle(SpeedTestTheme.Colors.Upload, SpeedTestTheme.Icons.Upload, SpeedTestTheme.Units.Mbps),
                _ => new PhaseStyle(SpeedTestTheme.Colors.Text, "", "")
            };
    }
}
