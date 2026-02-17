namespace Loupedeck.SpeedTestPlugin
{
    using System;

    using Loupedeck.SpeedTestPlugin.Helpers;

    public class SpeedTestPlugin : Plugin
    {
        public override Boolean UsesApplicationApiOnly => true;

        public override Boolean HasNoApplication => true;

        public SpeedTestPlugin()
        {
            PluginLog.Init(this.Log);
            PluginResources.Init(this.Assembly);
        }

        public override void Load() => PluginInstaller.EnsureInstalled(this.GetPluginDataDirectory(), this.Assembly);

        public override Boolean Uninstall() => PluginInstaller.Uninstall(this.GetPluginDataDirectory());
    }
}