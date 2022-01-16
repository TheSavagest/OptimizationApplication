namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;
using static System.Math;

// https://www.sfu.ca/~ssurjano/ackley.html
internal sealed class AckleyND : FunctionND
{
    public override string Name => FunctionName.Ackley;

    private const double a = 20;
    private const double b = 0.2;
    private const double c = 2 * PI;

    public AckleyND(
        byte dimension
    ) : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -32.768),
        Vector<double>.Build.Dense(dimension, 32.768))
    {
    }

    public override Function Copy()
    {
        return new AckleyND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        var sqrSum = coordinates.PointwisePower(2).Sum();
        var cosSum = coordinates.Multiply(c).PointwiseCos().Sum();

        return -a * Exp(-b * Sqrt(sqrSum / Dimension)) - Exp(cosSum / Dimension) + a + E;
    }
}

