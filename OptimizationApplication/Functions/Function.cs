using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using static OptimizationApplication.Vectors.DoubleVector.Create;
using static OptimizationApplication.Vectors.IntVector.Create;

namespace OptimizationApplication.Functions
{
    public abstract class Function
    {
        public string Titel { get; }
        public bool IsMinimization { get; }
        public int Dimension { get; }
        public DoubleVector OptimalCoordinates { get; protected set; }
        public DoubleVector LowerSearchBorders { get; }
        public DoubleVector UpperSearchBorders { get; }
        public IntVector GeneLengths { get; }
        public DoubleVector DiscretizationPeriod { get; }

        protected Function(string name, int dimension)
        {
#if DEBUG
            if (dimension < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension), dimension, $"The argument {nameof(dimension)} must be greater than 0.");
            }
#endif
            Titel = $"{name}_{dimension}D";
            IsMinimization = true;
            Dimension = dimension;
            OptimalCoordinates = Repeat(dimension, 0.0);
            LowerSearchBorders = Repeat(dimension, -10.0);
            UpperSearchBorders = Repeat(dimension, 10.0);
            GeneLengths = Repeat(dimension, 15);
            DiscretizationPeriod = Repeat(dimension, 0.00061037019);
        }

        public abstract double ValueFunction(DoubleVector coordinates);
    }
}
