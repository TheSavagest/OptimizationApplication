namespace OptimizationApplication.Extensions;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

public static class VectorDoubleBuilderExtensions
{
    private static readonly RandomSource randomSource = new Xoshiro256StarStar(13031997, true);

    internal static Vector<double> Uniform(
        this VectorBuilder<double> it,
        int length)
    {
        var tmpArray = new double[length];
        randomSource.NextDoubles(tmpArray);
        return it.Dense(tmpArray);
    }

    internal static Vector<double> Random(
        this VectorBuilder<double> it,
        int length,
        double min,
        double max)
    {
        if (max <= min) throw new ArgumentException("max <= min");

        return it.Uniform(length).Multiply(max - min).Add(min);
    }
}
