using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static System.Math;

namespace OptimizationApplication.Functions
{
    //http://benchmarkfcns.xyz/benchmarkfcns/happycatfcn.html
    public sealed class HappyCat : Function
    {
        public HappyCat(int dimension) : base("HappyCat", dimension)
        {
        }

        public override double ValueFunction(DoubleVector coordinates)
        {
#if DEBUG
            if (coordinates.Count != Dimension)
            {
                throw new ArgumentException($"The {nameof(coordinates)} is not equal function {nameof(Dimension)}.", nameof(coordinates));
            }
#endif
            double sum = coordinates.Sum();
            double sqrSum = coordinates.Sqr().Sum();

            return Pow(Pow(sqrSum - Dimension, 2), 0.125) + (0.5 * sqrSum + sum) / Dimension + 0.5;
        }
    }
}
