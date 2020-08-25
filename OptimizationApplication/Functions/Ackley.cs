using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static System.Math;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/ackley.html
    public sealed class Ackley : Function
    {
        private const double a = 20.0;
        private const double b = 0.2;
        private const double c = 2.0 * PI;

        public Ackley(int dimension) : base("Ackley", dimension)
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
            double sumSqr = coordinates.Sqr().Sum();
            double sumCos = (c * coordinates).Cos().Sum();

            return -a * Exp(-b * Sqrt(sumSqr / Dimension)) - Exp(sumCos / Dimension) + a + E;
        }
    }
}
