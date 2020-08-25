using static System.Math;

namespace OptimizationApplication.Extensions
{
    public static class DoubleExtensions
    {
        public const double DoubleDelta = 1E-12;

        public static bool IsEqualWithDelta(double value1, double value2, double delta = DoubleDelta)
        {
            return Abs(value1 - value2) < delta;
        }
    }
}
