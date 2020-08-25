using OptimizationApplication.Vectors;
using System.Linq;
using static OptimizationApplication.Extensions.StructExtensions;
using static OptimizationApplication.Vectors.IntVector.Create;
using static OptimizationApplication.Vectors.LongVector.Create;

namespace OptimizationApplication.Statistics
{
    public sealed class FunctionStatistic
    {
        private string FunctionName { get; }
        public double SuccessRate { get; }
        private int MinIteration { get; }
        private double AvgIteration { get; }
        private int MaxIteration { get; }
        private int MinEvaluation { get; }
        public double AvgEvaluation { get; }
        private int MaxEvaluation { get; }
        private long MinTimeMs { get; }
        public double AvgTimeMs { get; }
        private long MaxTimeMs { get; }

        public const string Header = "FunctionName;SuccessRate;MinIteration;AvgIteration;MaxIteration;MinEvaluation;AvgEvaluation;MaxEvaluation;MinTimeMs;AvgTimeMs;MaxTimeMs";

        public FunctionStatistic(string functionName, RunStatistic[] runStatistics)
        {
            FunctionName = functionName;

            RunStatistic[] successRuns = runStatistics.Where(runStatistic => StructOrException(runStatistic.IsSuccess))
                                                      .ToArray();

            if (successRuns.Length == 0)
            {
                SuccessRate = 0.0;

                MinIteration = 0;
                AvgIteration = 0;
                MaxIteration = 0;

                MinEvaluation = 0;
                AvgEvaluation = 0;
                MaxEvaluation = 0;

                MinTimeMs = 0;
                AvgTimeMs = 0;
                MaxTimeMs = 0;
            }
            else
            {
                SuccessRate = successRuns.Length / (double)runStatistics.Length;

                IntVector successIterations = FromArray(successRuns.Select(successRun => successRun.Iteration)
                                                                   .Select(StructOrException)
                                                                   .ToArray());
                MinIteration = successIterations.Min();
                AvgIteration = successIterations.Sum() / (double)successIterations.Count;
                MaxIteration = successIterations.Max();

                IntVector successEvaluation = FromArray(successRuns.Select(successRun => successRun.Evaluation)
                                                                   .Select(StructOrException)
                                                                   .ToArray());
                MinEvaluation = successEvaluation.Min();
                AvgEvaluation = successEvaluation.Sum() / (double)successEvaluation.Count;
                MaxEvaluation = successEvaluation.Max();

                LongVector successTimeMs = FromArray(successRuns.Select(successRun => successRun.TimeMs)
                                                                .Select(StructOrException)
                                                                .ToArray());
                MinTimeMs = successTimeMs.Min();
                AvgTimeMs = successTimeMs.Sum() / (double)successTimeMs.Count;
                MaxTimeMs = successTimeMs.Max();
            }
        }

        public override string ToString()
        {
            return $"{FunctionName};{SuccessRate};{MinIteration};{AvgIteration};{MaxIteration};{MinEvaluation};{AvgEvaluation};{MaxEvaluation};{MinTimeMs};{AvgTimeMs};{MaxTimeMs}";
        }
    }
}
