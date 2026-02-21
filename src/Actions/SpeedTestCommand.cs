namespace Loupedeck.SpeedTestPlugin.Actions
{
    using System;
    using System.Threading.Tasks;

    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;
    using Loupedeck.SpeedTestPlugin.Rendering;
    using Loupedeck.SpeedTestPlugin.Services;

    public class SpeedTestCommand : PluginDynamicCommand
    {
        private SpeedTestState _state;
        private readonly SpeedTestService _service;
        private readonly SpeedTestRenderer _renderer;

        public SpeedTestCommand() : base(displayName: PluginConstants.CommandDisplayName, description: PluginConstants.CommandDescription, groupName: "")
        {
            this.IsWidget = true;
            this._state = new SpeedTestState();
            this._service = new SpeedTestService();
            this._renderer = new SpeedTestRenderer();
        }

        protected override void RunCommand(String actionParameter)
        {
            PluginLog.Info("Speed test triggered.");
            Task.Run(async () =>
            {
                var progress = new Progress<SpeedTestState>(state =>
                {
                    this._state = state;
                    this.ActionImageChanged();
                });

                var pluginDataDirectory = this.Plugin.GetPluginDataDirectory();
                await this._service.RunTest(progress, pluginDataDirectory);
            });
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) => " ";

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize) => this._renderer.Render(this._state, imageSize);
    }
}