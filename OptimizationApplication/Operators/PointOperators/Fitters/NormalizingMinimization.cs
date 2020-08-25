#if DEBUG
using OptimizationApplication.Exceptions;
#endif
using OptimizationApplication.Solutions;
using static OptimizationApplication.Extensions.StructExtensions;

namespace OptimizationApplication.Operators.PointOperators.Fitters
{
    public sealed class NormalizingMinimization<TSolution> : PointFitter<TSolution> where TSolution : Point
    {
        public NormalizingMinimization() : base()
        {
        }

        public override PointFitter<TSolution> Copy()
        {
            return new NormalizingMinimization<TSolution>
            {
                IsMinimization = IsMinimization
            };
        }

        public override void Fit(params TSolution[] population)
        {
#if DEBUG
            if (IsMinimization == null)
            {
                throw new NotInitializedException(nameof(IsMinimization));
            }
#endif
            bool isMinimization = (bool)IsMinimization;
            double tmpValue;

            for (int i = 0; i < population.Length; i++)
            {
                tmpValue = StructOrException(population[i].Value);
                population[i].Fitness = isMinimization ? 1.0 / (1.0 + tmpValue) : tmpValue;
            }
        }
    }
}
