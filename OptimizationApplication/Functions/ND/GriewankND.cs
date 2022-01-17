namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;

// https://www.sfu.ca/~ssurjano/griewank.html
internal sealed class GriewankND : FunctionND
{
    private readonly Vector<double> sqrtRange;

    public GriewankND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -600),
        Vector<double>.Build.Dense(dimension, 600))
    {
        sqrtRange = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension)).PointwiseSqrt();
    }

    public override string Name => FunctionName.Griewank;

    public override Function Copy()
    {
        return new GriewankND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        var sqrSum = coordinates.PointwisePower(2).Sum();
        var cosMul = coordinates.PointwiseDivide(sqrtRange).PointwiseCos().Mul();

        return (sqrSum / 4000.0) - cosMul + 1;
    }
}
