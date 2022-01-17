namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using static System.Math;

// https://www.sfu.ca/~ssurjano/ackley.html
internal sealed class AckleyND : FunctionND
{
    private const double A = 20;
    private const double B = 0.2;
    private const double C = 2 * PI;

    public AckleyND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -32.768),
        Vector<double>.Build.Dense(dimension, 32.768))
    {
    }

    public override string Name => FunctionName.Ackley;

    public override Function Copy()
    {
        return new AckleyND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        var sqrSum = coordinates.PointwisePower(2).Sum();
        var cosSum = coordinates.Multiply(C).PointwiseCos().Sum();

        return (-A * Exp(-B * Sqrt(sqrSum / Dimension))) - Exp(cosSum / Dimension) + A + E;
    }
}
