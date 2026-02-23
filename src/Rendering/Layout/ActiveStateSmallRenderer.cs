namespace Loupedeck.SpeedTestPlugin.Rendering.Layout
{
    using System;

    using Loupedeck.SpeedTestPlugin.Constants;
    using Loupedeck.SpeedTestPlugin.Helpers;
    using Loupedeck.SpeedTestPlugin.Models;
    using Loupedeck.SpeedTestPlugin.Rendering.Providers;

    public class ActiveStateSmallRenderer(IPhaseStyleProvider phaseStyleProvider) : IStateRenderer
    {
        private readonly IPhaseStyleProvider _phaseStyleProvider = phaseStyleProvider;

        public Boolean CanRender(SpeedTestState state, DisplayFormat format) => state.IsActive && format == DisplayFormat.Small;

        public void Render(ImageBuilder builder, SpeedTestState state)
        {
            var phaseStyle = this._phaseStyleProvider.GetPhaseStyle(state);
            var text = $"{phaseStyle.Icon}{state.Speed}";
            var y = (Int32)(SpeedTestTheme.Dimensions.ReferenceResolution * 0.34);
            builder.DrawHorizontallyCenteredText(text, SpeedTestTheme.Fonts.Large, phaseStyle.Color, y, SpeedTestTheme.Dimensions.ReferenceResolution);
        }
    }
}
