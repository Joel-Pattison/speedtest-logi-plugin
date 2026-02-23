namespace Loupedeck.SpeedTestPlugin.Models
{
    using System;

    public class SpeedTestState
    {
        public SpeedTestPhase Phase { get; set; } = SpeedTestPhase.Ready;
        public String Speed { get; set; } = "";
        public String Progress { get; set; } = "";
        public String ServerLocation { get; set; } = "";

        public Boolean IsReady => this.Phase == SpeedTestPhase.Ready;
        public Boolean IsDone => this.Phase == SpeedTestPhase.Done;
        public Boolean IsError => this.Phase == SpeedTestPhase.Error;
        public Boolean IsActive => this.Phase is SpeedTestPhase.Ping or SpeedTestPhase.Download or SpeedTestPhase.Upload;

        public String[] GetSpeedParts() => this.Speed.Split('/');
    }
}
