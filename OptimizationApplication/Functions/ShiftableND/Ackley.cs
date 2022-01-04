namespace OptimizationApplication.Functions.ShiftableND;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using static System.Math;

//https://www.sfu.ca/~ssurjano/ackley.html
public sealed class Ackley : ShiftableNDFunction
{
    public override string Name => FunctionName.Ackley;

    public Ackley(
        byte dimension,
        bool shift
    ) : base(
        dimension,
        shift)
    {
    }

    private Ackley(
        byte dimension,
        Vector<double> optimum,
        Vector<double> lower,
        Vector<double> upper
    ) : base(
        dimension,
        optimum,
        lower,
        upper)
    {
    }

    public override Function Copy()
    {
        return new Ackley(
            Dimension,
            Optimum.Clone(),
            Lower.Clone(),
            Upper.Clone());
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));

        const double a = 20;
        const double b = 0.2;
        const double c = Constants.Pi2;

        var tmpX = coordinates.Subtract(Optimum);
        var sqrSum = tmpX.PointwisePower(2).Sum();
        var cosSum = tmpX.Multiply(c).PointwiseCos().Sum();

        return -a * Exp(-b * Sqrt(sqrSum / Dimension)) - Exp(cosSum / Dimension) + a + E;
    }
}
