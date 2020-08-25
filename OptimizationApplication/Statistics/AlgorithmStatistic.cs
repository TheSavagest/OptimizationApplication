using System.Linq;

namespace OptimizationApplication.Statistics
{
    public sealed class AlgorithmStatistic
    {
        public string AlgorithmName { get; }
        public string[] SuccessRates { get; }
        public string[] AverageEvaluations { get; }
        public string[] AverageTimeMs { get; }

        public AlgorithmStatistic(string algorithmName, FunctionStatistic[] functionStatistics)
        {
            AlgorithmName = algorithmName;
            SuccessRates = functionStatistics.Select(functionStatistic => $"{functionStatistic.SuccessRate:0.00}")
                                             .ToArray();
            AverageEvaluations = functionStatistics.Select(functionStatistic => $"{functionStatistic.AvgEvaluation:0.00}")
                                                   .ToArray();
            AverageTimeMs = functionStatistics.Select(functionStatistic => $"{functionStatistic.AvgTimeMs:0.00}")
                                              .ToArray();
        }
    }
}
