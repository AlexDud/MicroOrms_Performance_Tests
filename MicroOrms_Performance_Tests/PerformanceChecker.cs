using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MicroOrms_Performance_Tests
{
    public static class PerformanceChecker
    {
        public static double check_average_execution_time(Action action)
        {
            var list = new List<long>();
            var sw = new Stopwatch();

            for (int j = 0; j < 20; j++)
            {
                sw.Reset();
                sw.Start();
                for (int i = 0; i < 50; i++)
                {
                    action();
                }
                sw.Stop();
                list.Add(sw.ElapsedMilliseconds);
            }

            return list.Average();
        }
    }
}
