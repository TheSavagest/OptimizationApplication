namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;
using static System.Math;

// https://www.sfu.ca/~ssurjano/zakharov.html
internal sealed class ZakharovND : FunctionND
{
    public override string Name => FunctionName.Zakharov;
    private readonly Vector<double> Range;

    public ZakharovND(
        byte dimension
    ) : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -5),
        Vector<double>.Build.Dense(dimension, 10))
    {
        Range = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension));
    }

    public override Function Copy()
    {
        return new ZakharovND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        var sumSqr = coordinates.PointwisePower(2).Sum();
        var sumRange = 0.5 * coordinates.PointwiseMultiply(Range).Sum();

        return sumSqr + Pow(sumRange, 2) + Pow(sumRange, 4);
    }
}
