namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

// https://www.sfu.ca/~ssurjano/sumsqu.html
internal sealed class SumSquaresND : FunctionND
{
    private readonly Vector<double> range;

    public SumSquaresND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -10),
        Vector<double>.Build.Dense(dimension, 10))
    {
        range = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension));
    }

    public override string Name => FunctionName.SumSquares;

    public override Function Copy()
    {
        return new SumSquaresND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        return coordinates
            .PointwisePower(2)
            .PointwiseMultiply(range)
            .Sum();
    }
}
