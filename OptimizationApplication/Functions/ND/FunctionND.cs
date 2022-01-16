namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;

internal abstract class FunctionND : Function
{
    public override string Type => FunctionType.ND;

    protected FunctionND(
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

