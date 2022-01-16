using MathNet.Numerics.LinearAlgebra;

namespace OptimizationApplication.Extensions;

internal static class VectorDoubleExtensions
{
    internal static bool IsAllLessThan(
        this Vector<double> it,
        Vector<double> other)
    {
        return it.ForAll2((it, other) => it < other, other, Zeros.Include);
    }

    private static bool IsAllGreaterThan(
        this Vector<double> it,
        Vector<double> other)
    {
        return it.ForAll2((it, other) => it > other, other, Zeros.Include);
    }

    internal static bool IsAllInInterval(
        this Vector<double> it,
        Vector<double> mins,
        Vector<double> maxs)
    {
        return it.IsAllLessThan(maxs) && it.IsAllGreaterThan(mins);
    }

    internal static double Mul(
        this Vector<double> it)
    {
        return it.Aggregate(1.0, (acc, val) => acc * val);
    }
}
