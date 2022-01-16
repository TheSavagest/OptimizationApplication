namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;
using static System.Math;

// https://www.sfu.ca/~ssurjano/levy.html
internal sealed class LevyND : FunctionND
{
    public override string Name => FunctionName.Levy;

    public LevyND(
        byte dimension
    ) : base(
        dimension,
        Vector<double>.Build.Dense(dimension, 1),
        Vector<double>.Build.Dense(dimension, -10),
        Vector<double>.Build.Dense(dimension, 10))
    {
    }

    public override Function Copy()
    {
        return new LevyND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        var w = coordinates.Subtract(1).Divide(4).Add(1);
        var wSubVector = w.SubVector(0, Dimension - 1);
        var sum = wSubVector.Multiply(PI).Add(1).PointwiseSin().PointwisePower(2).Multiply(10).Add(1).PointwiseMultiply(
            wSubVector.Subtract(1).PointwisePower(2)).Sum();

        return Pow(Sin(PI * w[0]), 2) + sum + Pow(w[^1] - 1, 2) * (1 + Pow(Sin(2 * PI * w[^1]), 2));
    }
}
