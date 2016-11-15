using System;
using BenchmarkDotNet.Running;
using MicroOrms_Performance_Tests.BenchMarks;

namespace MicroOrms_Performance_Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run<InsertBenchMarks>();
            BenchmarkRunner.Run<UpdateBenchMarks>();
            //BenchmarkRunner.Run<SelectBenchMarks>();

            Console.ReadKey();
        }
    }
}
