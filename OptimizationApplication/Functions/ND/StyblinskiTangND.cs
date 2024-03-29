﻿namespace OptimizationApplication.Functions.ND;

using MathNet.Numerics.LinearAlgebra;

// http://infinity77.net/global_optimization/test_functions_nd_S.html#go_benchmark.StyblinskiTang
internal sealed class StyblinskiTangND : FunctionND
{
    public StyblinskiTangND(
        byte dimension)
    : base(
        dimension,
        Vector<double>.Build.Dense(dimension, -2.90353401818596),
        Vector<double>.Build.Dense(dimension, -5),
        Vector<double>.Build.Dense(dimension, 5))
    {
    }

    public override string Name => FunctionName.Sphere;

    public override Function Copy()
    {
        return new StyblinskiTangND(Dimension);
    }

    public override double GetValue(Vector<double> coordinates)
    {
        CheckCoordinates(coordinates);

        var sum = coordinates.Sum();
        var sum2 = coordinates.PointwisePower(2).Sum();
        var sum4 = coordinates.PointwisePower(4).Sum();

        return (0.5 * sum4) - (8 * sum2) + (2.5 * sum);
    }
}
