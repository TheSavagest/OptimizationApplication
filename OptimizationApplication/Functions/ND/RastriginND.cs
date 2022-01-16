namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;
using static System.Math;

// https://www.sfu.ca/~ssurjano/rastr.html
internal sealed class RastriginND : FunctionND
{
    public override string Name => FunctionName.Rastrigin;

    public RastriginND(
        byte dimension
    ) : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -5.12),
        Vector<double>.Build.Dense(dimension, 5.12))
    {
    }

    public override Function Copy()
    {
        return new RastriginND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        var sumSqr = coordinates.PointwisePower(2).Sum();
        var sumCos = coordinates.Multiply(2 * PI).PointwiseCos().Sum();

        return 10 * Dimension + sumSqr - 10 * sumCos;
    }
}
