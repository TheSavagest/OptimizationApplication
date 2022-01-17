namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using static System.Math;

// https://www.sfu.ca/~ssurjano/zakharov.html
internal sealed class ZakharovND : FunctionND
{
    private readonly Vector<double> range;

    public ZakharovND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -5),
        Vector<double>.Build.Dense(dimension, 10))
    {
        range = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension));
    }

    public override string Name => FunctionName.Zakharov;

    public override Function Copy()
    {
        return new ZakharovND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        var sumSqr = coordinates.PointwisePower(2).Sum();
        var sumRange = 0.5 * coordinates.PointwiseMultiply(range).Sum();

        return sumSqr + Pow(sumRange, 2) + Pow(sumRange, 4);
    }
}
