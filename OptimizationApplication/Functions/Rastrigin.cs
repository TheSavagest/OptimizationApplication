using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static System.Math;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/rastr.html
    public sealed class Rastrigin : Function
    {
        private const double A = 10.0;
        private const double PI2 = 2.0 * PI;

        public Rastrigin(int dimension) : base("Rastrigin", dimension)
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
            return A * Dimension + (coordinates.Sqr() - A * (PI2 * coordinates).Cos()).Sum();
        }
    }
}
