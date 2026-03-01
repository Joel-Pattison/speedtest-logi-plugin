namespace Loupedeck.SpeedTestPlusPlugin
{
    using System;

    using Loupedeck.SpeedTestPlusPlugin.Helpers;

    public class SpeedTestPlusPlugin : Plugin
    {
        public override Boolean UsesApplicationApiOnly => true;

        public override Boolean HasNoApplication => true;

        public SpeedTestPlusPlugin()
        {
            PluginLog.Init(this.Log);
            PluginResources.Init(this.Assembly);
        }

        public override void Load() => PluginInstaller.EnsureInstalled(this.GetPluginDataDirectory(), this.Assembly);

        public override Boolean Uninstall() => PluginInstaller.Uninstall(this.GetPluginDataDirectory());
    }
}
