using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/rosen.html
    public sealed class Rosenbrock : Function
    {
        public Rosenbrock(int dimension) : base("Rosenbrock", dimension)
        {
            OptimalCoordinates = Repeat(dimension, 1.0);
        }

        public override double ValueFunction(DoubleVector coordinates)
        {
#if DEBUG
            if (coordinates.Count != Dimension)
            {
                throw new ArgumentException($"The {nameof(coordinates)} is not equal function {nameof(Dimension)}.", nameof(coordinates));
            }
#endif
            DoubleVector xn = coordinates.SubVector(1, Dimension - 1);
            DoubleVector xp = coordinates.SubVector(0, Dimension - 1);

            return (100.0 * (xn - xp.Sqr()).Sqr() + (xp - 1.0).Sqr()).Sum();
        }
    }
}
