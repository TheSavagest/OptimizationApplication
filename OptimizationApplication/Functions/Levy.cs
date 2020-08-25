using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static OptimizationApplication.Vectors.DoubleVector.Create;
using static System.Math;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/levy.html
    public sealed class Levy : Function
    {
        public Levy(int dimension) : base("Levy", dimension)
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
            DoubleVector w = 1.0 + (coordinates - 1.0) / 4.0;

            double first = Pow(Sin(PI * w.First), 2);

            DoubleVector subVector = w.SubVector(0, Dimension - 1);
            double sum = ((subVector - 1.0).Sqr() * (1.0 + 10.0 * (1.0 + PI * subVector).Sin().Sqr())).Sum();

            double last = Pow(w.Last - 1.0, 2) * (1.0 + Pow(Sin(2 * PI * w.Last), 2.0));

            return first + sum + last;
        }
    }
}
