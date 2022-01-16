namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;

// https://www.sfu.ca/~ssurjano/sumsqu.html
internal sealed class SumSquaresND : FunctionND
{
    public override string Name => FunctionName.SumSquares;
    private readonly Vector<double> Range;

    public SumSquaresND(
        byte dimension
    ) : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -10),
        Vector<double>.Build.Dense(dimension, 10))
    {
        Range = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension));
    }

    public override Function Copy()
    {
        return new SumSquaresND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        return coordinates.PointwisePower(2).PointwiseMultiply(Range).Sum();
    }
}
