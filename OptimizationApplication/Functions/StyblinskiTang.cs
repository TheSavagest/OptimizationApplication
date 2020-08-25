using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/stybtang.html
    public sealed class StyblinskiTang : Function
    {
        public StyblinskiTang(int dimension) : base("StyblinskiTang", dimension)
        {
            OptimalCoordinates = Repeat(dimension, -2.90353402777118);
        }

        public override double ValueFunction(DoubleVector coordinates)
        {
#if DEBUG
            if (coordinates.Count != Dimension)
            {
                throw new ArgumentException($"The {nameof(coordinates)} is not equal function {nameof(Dimension)}.", nameof(coordinates));
            }
#endif
            return 0.5 * (coordinates.Sqr().Sqr() - 16.0 * coordinates.Sqr() + 5.0 * coordinates).Sum() + 39.1661657037714 * Dimension;
        }
    }
}
