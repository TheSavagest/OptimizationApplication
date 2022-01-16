namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;

// https://www.sfu.ca/~ssurjano/griewank.html
internal sealed class GriewankND : FunctionND
{
    public override string Name => FunctionName.Griewank;
    private readonly Vector<double> SqrtRange;

    public GriewankND(
        byte dimension
    ) : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -600),
        Vector<double>.Build.Dense(dimension, 600))
    {
        SqrtRange = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension)).PointwiseSqrt();
    }

    public override Function Copy()
    {
        return new GriewankND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        var sqrSum = coordinates.PointwisePower(2).Sum();
        var cosMul = coordinates.PointwiseDivide(SqrtRange).PointwiseCos().Mul();

        return sqrSum / 4000.0 - cosMul + 1;
    }
}
