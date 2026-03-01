namespace Loupedeck.SpeedTestPlusPlugin.Services
{
    using System;
    using System.Threading.Tasks;

    using Loupedeck.SpeedTestPlusPlugin.Models;

    public interface ISpeedTestService
    {
        Task RunTest(IProgress<SpeedTestState> progress, String pluginDataDirectory);
    }
}
