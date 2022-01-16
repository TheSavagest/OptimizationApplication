namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;

// https://www.sfu.ca/~ssurjano/rosen.html
internal sealed class RosenbrockND : FunctionND
{
    public override string Name => FunctionName.Rastrigin;

    public RosenbrockND(
        byte dimension
    ) : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -5),
        Vector<double>.Build.Dense(dimension, 10))
    {
    }

    public override Function Copy()
    {
        return new RosenbrockND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        var xi = coordinates.SubVector(0, Dimension - 1);
        var xn = coordinates.SubVector(1, Dimension - 1);

        var sumDiff = 100 * xi.PointwisePower(2).Negate().Add(xn).PointwisePower(2).Sum();
        var sumSqr = xi.Subtract(1).PointwisePower(2).Sum();

        return sumDiff + sumSqr;
    }
}
