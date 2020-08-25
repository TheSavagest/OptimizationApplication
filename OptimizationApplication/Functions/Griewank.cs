using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Functions
{
    //https://www.sfu.ca/~ssurjano/griewank.html
    public sealed class Griewank : Function
    {
        private readonly DoubleVector SqrtIndexes;

        public Griewank(int dimension) : base("Griewank", dimension)
        {
            SqrtIndexes = Sequence(1.0, 1.0, dimension).Sqrt();
        }

        public override double ValueFunction(DoubleVector coordinates)
        {
#if DEBUG
            if (coordinates.Count != Dimension)
            {
                throw new ArgumentException($"The {nameof(coordinates)} is not equal function {nameof(Dimension)}.", nameof(coordinates));
            }
#endif
            double sumSqr = coordinates.Sqr().Sum() / 4000.0;
            double mulCos = (coordinates / SqrtIndexes).Cos().Mul();

            return sumSqr - mulCos + 1.0;
        }
    }
}
