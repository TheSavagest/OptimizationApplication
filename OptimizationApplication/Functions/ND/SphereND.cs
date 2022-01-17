namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;

// https://www.sfu.ca/~ssurjano/spheref.html
internal sealed class SphereND : FunctionND
{
    public SphereND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension),
        Vector<double>.Build.Dense(dimension, -5.12),
        Vector<double>.Build.Dense(dimension, 5.12))
    {
    }

    public override string Name => FunctionName.Sphere;

    public override Function Copy()
    {
        return new SphereND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        return coordinates.PointwisePower(2).Sum();
    }
}
