using OptimizationApplication.Exceptions;
using OptimizationApplication.Solutions;
using OptimizationApplication.Vectors;
using System.Linq;
using static OptimizationApplication.Extensions.StructExtensions;
using static OptimizationApplication.Vectors.DoubleVector;
using static OptimizationApplication.Vectors.DoubleVector.Create;
using static System.Math;

namespace OptimizationApplication.Statistics
{
    public sealed class IterationStatistic<TSolution> where TSolution : Point
    {
        private int Iteration { get; }
        private double BestDistance { get; }
        private double MinDistance { get; }
        private double AvgDistance { get; }
        private double MaxDistance { get; }
        private double BestValue { get; }
        private double MinValue { get; }
        private double AvgValue { get; }
        private double MaxValue { get; }

        public const string Header = "Iteration;BestDistance;MinDistance;AvgDistance;MaxDistance;BestValue;MinValue;AvgValue;MaxValue";

        public IterationStatistic(int iteration, TSolution[] population, TSolution best, DoubleVector optimalCoordinates)
        {
            Iteration = iteration;

            DoubleVector bestCoordinates = best.Coordinates ?? throw new NotInitializedException(nameof(best.Coordinates));
            BestDistance = Distance(bestCoordinates, optimalCoordinates);
            BestValue = best.Value ?? throw new NotInitializedException(nameof(best.Value));

            DoubleVector distances = FromArray(population.Select(ind => ind.Coordinates)
                                                         .Select(StructOrException)
                                                         .Select(coords => Distance(coords, optimalCoordinates))
                                                         .ToArray());
            MinDistance = distances.Min();
            AvgDistance = distances.Sum() / distances.Count;
            MaxDistance = distances.Max();

            DoubleVector values = FromArray(population.Select(ind => ind.Value)
                                                      .Select(StructOrException)
                                                      .ToArray());
            MinValue = values.Min();
            AvgValue = values.Sum() / values.Count;
            MaxValue = values.Max();
        }

        public override string ToString()
        {
            return $"{Iteration};{BestDistance};{MinDistance};{AvgDistance};{MaxDistance};{BestValue};{MinValue};{AvgValue};{MaxValue}";
        }
    }
}
