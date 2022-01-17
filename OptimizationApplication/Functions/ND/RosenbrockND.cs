namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;

// https://www.sfu.ca/~ssurjano/rosen.html
internal sealed class RosenbrockND : FunctionND
{
    public RosenbrockND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -5),
        Vector<double>.Build.Dense(dimension, 10))
    {
    }

    public override string Name => FunctionName.Rastrigin;

    public override Function Copy()
    {
        return new RosenbrockND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        var xi = coordinates.SubVector(0, Dimension - 1);
        var xn = coordinates.SubVector(1, Dimension - 1);

        var sumDiff = xi
            .PointwisePower(2)
            .Negate()
            .Add(xn)
            .PointwisePower(2)
            .Sum();
        var sumSqr = xi
            .Subtract(1)
            .PointwisePower(2)
            .Sum();

        return (100 * sumDiff) + sumSqr;
    }
}
