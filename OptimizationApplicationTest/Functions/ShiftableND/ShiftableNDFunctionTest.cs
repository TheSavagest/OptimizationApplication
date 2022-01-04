namespace OptimizationApplicationTest.Functions.ShiftableND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Functions.ShiftableND;
using System.Linq;
using Xunit;

public static class ShiftableNDFunctionTest
{
    [Fact]
    public static void Constructor_ShiftOptimum_If_ShiftArgumentPassed()
    {
        const byte dimension = 2;
        var ackley = new Ackley(dimension, true);
        var zeroVector = Vector<double>.Build.Dense(dimension);
        Assert.NotEqual(zeroVector, ackley.Optimum);
    }

    [Fact]
    public static void Optimum_Contains_NegativeValue_If_ShiftArgumentPassed()
    {
        var ackley = new Ackley(byte.MaxValue, true);
        Assert.Contains(ackley.Optimum, it => it < 0);
    }

    [Fact]
    public static void Lower_ContainsOnly_NegativesValues()
    {
        var ackley = new Ackley(byte.MaxValue, true);
        Assert.True(ackley.Lower.All(x => x < 0));
    }
}
