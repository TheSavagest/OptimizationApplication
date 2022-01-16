namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;

// https://www.sfu.ca/~ssurjano/spheref.html
internal sealed class SphereND : FunctionND
{
    public override string Name => FunctionName.Sphere;

    public SphereND(
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
        return new SphereND(Dimension);
    }

    public override double GetValue(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        if (!coordinates.IsAllInInterval(Lower, Upper))
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

        return coordinates.PointwisePower(2).Sum();
    }
}
