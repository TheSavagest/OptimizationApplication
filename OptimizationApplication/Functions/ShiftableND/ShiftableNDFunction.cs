namespace OptimizationApplication.Functions.ShiftableND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;

public abstract class ShiftableNDFunction : Function
{
    public override string Type => FunctionType.ShiftableND;

    protected ShiftableNDFunction(
        byte dimension,
        bool shift
    ) : base(
        dimension,
        shift ? Vector<double>.Build.Random(dimension, -80, 80) : Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -100),
        Vector<double>.Build.Dense(dimension, 100))
    {
    }

    protected ShiftableNDFunction(
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
}
