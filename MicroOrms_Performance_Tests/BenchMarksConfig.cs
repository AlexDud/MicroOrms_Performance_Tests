using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace MicroOrms_Performance_Tests
{
    public class BenchMarksConfig : ManualConfig
    {
        public BenchMarksConfig()
        {
            Add(new Job(EnvMode.LegacyJitX64, EnvMode.Clr, RunMode.Dry)
            {
                Env = { Runtime = Runtime.Clr, Platform = Platform.X64 },
                Run = { LaunchCount = 3, WarmupCount = 10, TargetCount = 100 },
                Accuracy = { MaxStdErrRelative = 0.01 }
            });
        }
    }
}