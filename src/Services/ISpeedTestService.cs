namespace Loupedeck.SpeedTestPlugin.Services
{
    using System;
    using System.Threading.Tasks;

    using Loupedeck.SpeedTestPlugin.Models;

    public interface ISpeedTestService
    {
        Task RunTest(IProgress<SpeedTestState> progress, String pluginDataDirectory);
    }
}
