namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using static System.Math;

// https://www.sfu.ca/~ssurjano/rastr.html
internal sealed class RastriginND : FunctionND
{
    public RastriginND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -5.12),
        Vector<double>.Build.Dense(dimension, 5.12))
    {
    }

    public override string Name => FunctionName.Rastrigin;

    public override Function Copy()
    {
        return new RastriginND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        var sumSqr = coordinates.PointwisePower(2).Sum();
        var sumCos = coordinates
            .Multiply(2 * PI)
            .PointwiseCos()
            .Sum();

        return (10 * Dimension) + sumSqr - (10 * sumCos);
    }
}
