using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/spheref.html
    public sealed class Sphere : Function
    {
        public Sphere(int dimension) : base("Sphere", dimension)
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
            return coordinates.Sqr().Sum();
        }
    }
}
