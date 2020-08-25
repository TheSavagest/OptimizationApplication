using OptimizationApplication.Algorithms;
using OptimizationApplication.Exceptions;
using OptimizationApplication.Functions;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
using OptimizationApplication.Statistics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OptimizationApplication.Testing
{
    public sealed class Tester<TSolution> where TSolution : Point, new()
    {
        private Writer Writer { get; }
        private int NumberOfRuns { get; }
        private TreadSafeRandom Random { get; }
        private bool IsParallelTesting { get; }

        private int ProgressCounter;
        private int MaxProgress;

        public Tester(Writer writer, int numberOfRuns, int randomSeed, bool isParallelTesting)
        {
            Writer = writer.Copy();
            NumberOfRuns = numberOfRuns;
            Random = new TreadSafeRandom(randomSeed);
            IsParallelTesting = isParallelTesting;
        }

        public void TestAll(Algorithm<TSolution>[] algorithms, Function[] functions)
        {
            ProgressCounter = 0;
            MaxProgress = algorithms.Length * functions.Length;

            AlgorithmStatistic[] algorithmsStatistics = new AlgorithmStatistic[algorithms.Length];

            for (int i = 0; i < algorithms.Length; i++)
            {
                algorithmsStatistics[i] = TestOne(algorithms[i], functions);
            }

            Writer.WriteAll($";{string.Join(';', functions.Select(function => function.Titel))}", algorithmsStatistics);
        }

        private AlgorithmStatistic TestOne(Algorithm<TSolution> algorithm, Function[] functions)
        {
            int functionsCount = functions.Length;
            FunctionStatistic[] functionStatistics = new FunctionStatistic[functionsCount];

            for (int i = 0; i < functionsCount; i++)
            {
                functionStatistics[i] = TestCase(algorithm, functions[i]);
            }

            Writer.Write($"{algorithm.Titel}", FunctionStatistic.Header, functionStatistics);

            return new AlgorithmStatistic(algorithm.Titel, functionStatistics);
        }

        private FunctionStatistic TestCase(Algorithm<TSolution> algorithm, Function function)
        {
            ProgressCounter++;
            Console.WriteLine($"{ProgressCounter}/{MaxProgress}");

            RunStatistic[] runStatistics = new RunStatistic[NumberOfRuns];

            if (IsParallelTesting)
            {
                Parallel.For(0, NumberOfRuns, i => runStatistics[i] = TestRun(algorithm, function, i));
            }
            else
            {
                for (int i = 0; i < NumberOfRuns; i++)
                {
                    runStatistics[i] = TestRun(algorithm, function, i);
                }
            }

            Writer.Write($"{algorithm.Titel}_{function.Titel}", RunStatistic.Header, runStatistics);

            return new FunctionStatistic(function.Titel, runStatistics);
        }

        private RunStatistic TestRun(Algorithm<TSolution> algorithm, Function function, int runIndex)
        {
            Console.WriteLine($"{algorithm.Titel}_{function.Titel}_{runIndex}");

            Algorithm<TSolution> runnableAlgorithm = algorithm.Copy();
            runnableAlgorithm.SetRunData(Random.Next(), runIndex, function);

            runnableAlgorithm.Run();

            if (runnableAlgorithm.IsCollectIterationStatistic)
            {
                IterationStatistic<TSolution>[] iterationsStatistics = algorithm.IterationsStatistics ?? throw new NotInitializedException("algorithm.IterationsStatistics");

                Writer.Write($"{runnableAlgorithm.Titel}_{function.Titel}_{runIndex}",
                             IterationStatistic<TSolution>.Header,
                             iterationsStatistics);
            }

            return runnableAlgorithm.RunStatistic;
        }
    }
}
