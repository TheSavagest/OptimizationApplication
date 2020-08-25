using OptimizationApplication.Vectors;

namespace OptimizationApplication.Solutions
{
    public abstract class Point : Solution
    {
        public double? Value { get; set; }
        public DoubleVector? Coordinates { get; set; }

        public Point() : base()
        {
            Value = null;
            Coordinates = null;
        }
    }
}
