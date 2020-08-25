using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/zakharov.html
    public sealed class Zakharov : Function
    {
        private readonly DoubleVector Multiplier;

        public Zakharov(int dimension) : base("Zakharov", dimension)
        {
            Multiplier = Sequence(0.5, 0.5, dimension);
        }

        public override double ValueFunction(DoubleVector coordinates)
        {
#if DEBUG
            if (coordinates.Count != Dimension)
            {
                throw new ArgumentException($"The {nameof(coordinates)} is not equal function {nameof(Dimension)}.", nameof(coordinates));
            }
#endif
            double sqrSum = coordinates.Sqr().Sum();
            double halfSum = (Multiplier * coordinates).Sum();

            return sqrSum + halfSum * halfSum * (1.0 + halfSum * halfSum);
        }
    }
}
