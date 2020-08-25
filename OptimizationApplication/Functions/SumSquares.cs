using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/sumsqu.html
    public sealed class SumSquares : Function
    {
        private readonly DoubleVector Indexes;

        public SumSquares(int dimension) : base("SumSquares", dimension)
        {
            Indexes = Sequence(1.0, 1.0, dimension);
        }

        public override double ValueFunction(DoubleVector coordinates)
        {
#if DEBUG
            if (coordinates.Count != Dimension)
            {
                throw new ArgumentException($"The {nameof(coordinates)} is not equal function {nameof(Dimension)}.", nameof(coordinates));
            }
#endif
            return (Indexes * coordinates.Sqr()).Sum();
        }
    }
}
