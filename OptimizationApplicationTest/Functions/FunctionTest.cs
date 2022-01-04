namespace OptimizationApplicationTest.Functions;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Functions;
using OptimizationApplication.Functions.ShiftableND;
using System;
using Xunit;

internal sealed class TestFunction : Function
{
    public override string Name => throw new NotImplementedException();

    public override string Type => throw new NotImplementedException();

    internal TestFunction(
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
        throw new NotImplementedException();
    }

    public override double GetValue(Vector<double> coordinates)
    {
        throw new NotImplementedException();
    }
}

public class FunctionTest
{
    [Fact]
    public void GetTitelReturnsCorrectValue()
    {
        var ackley = new Ackley(2, false);
        Assert.Equal("ShiftableND-Ackley2D", ackley.Titel);
    }

    [Fact]
    public static void Constructor_ThrewArgumentException_If_ArgumentDimenssion_IsEqualTo_Zero()
    {
        var action = () => new Ackley(0, false);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("dimension == 0 (Parameter 'dimension')", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrewArgumentException_If_ArgumentOptimumCount_IsNotEqualTo_ArgumnetDimension()
    {
        const byte dimension = 2;
        var optimum = Vector<double>.Build.Dense(dimension + 1);
        var lower = Vector<double>.Build.Dense(dimension);
        var upper = Vector<double>.Build.Dense(dimension);

        var action = () => new TestFunction(dimension, optimum, lower, upper);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("optimum.Count != dimension (Parameter 'optimum')", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrewArgumentException_If_ArgumentLowerCount_IsNotEqualTo_ArgumnetDimension()
    {
        const byte dimension = 2;
        var optimum = Vector<double>.Build.Dense(dimension);
        var lower = Vector<double>.Build.Dense(dimension + 1);
        var upper = Vector<double>.Build.Dense(dimension);

        var action = () => new TestFunction(dimension, optimum, lower, upper);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("lower.Count != dimension (Parameter 'lower')", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrewArgumentException_If_ArgumentUpperCount_IsNotEqualTo_ArgumnetDimension()
    {
        const byte dimension = 2;
        var optimum = Vector<double>.Build.Dense(dimension);
        var lower = Vector<double>.Build.Dense(dimension);
        var upper = Vector<double>.Build.Dense(dimension + 1);

        var action = () => new TestFunction(dimension, optimum, lower, upper);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("upper.Count != dimension (Parameter 'upper')", exception.Message);
    }

    [Fact]
    public static void GetHashCodeMethod_Returns_NotZero()
    {
        var function = new Ackley(2, false);
        var actual = function.GetHashCode();
        Assert.NotEqual(0, actual);
    }

    [Fact]
    public static void ToStringMethod_Returns_CorrectValue()
    {
        var expected = "ShiftableND-Ackley2D Optimum: [0, 0] Lower: [-100, -100] Upper: [100, 100]";
        var function = new Ackley(2, false);
        var actual = function.ToString();
        Assert.Equal(expected, actual);
    }
}
