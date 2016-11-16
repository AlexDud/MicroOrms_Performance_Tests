using System;
using BenchmarkDotNet.Running;
using MicroOrms_Performance_Tests.BenchMarks;

namespace MicroOrms_Performance_Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run<InsertBenchmark>();
            //BenchmarkRunner.Run<UpdateBenchmark>();
            //BenchmarkRunner.Run<SelectBenchmark>();
            //BenchmarkRunner.Run<TakeSkipBenchmark>();
            //BenchmarkRunner.Run<PaginationBenchmark>();
            BenchmarkRunner.Run<FetchSingleBenchmark>();

            Console.ReadKey();
        }
    }
}
