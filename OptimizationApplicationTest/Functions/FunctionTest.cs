namespace OptimizationApplicationTest.Functions;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Functions;
using OptimizationApplication.Functions.ND;
using System;
using Xunit;

internal sealed class TestFunction : FunctionND
{
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

    public override string Name => throw new NotImplementedException();
    public override Function Copy() => throw new NotImplementedException();
    public override double GetValue(Vector<double> coordinates) => throw new NotImplementedException();
}

public static class FunctionTest
{
    private static readonly Vector<double> Vector1d = Vector<double>.Build.Dense(1);
    private static readonly Vector<double> Vector2d00 = Vector<double>.Build.Dense(new double[] { 0, 0 });
    private static readonly Vector<double> Vector2d01 = Vector<double>.Build.Dense(new double[] { 0, 1 });
    private static readonly Vector<double> Vector2d11 = Vector<double>.Build.Dense(new double[] { 1, 1 });

    [Fact]
    public static void Function_TitelProperty_Returns_CorrectValue()
    {
        var function = new AckleyND(2);
        var actualTitel = function.Titel;
        var expectedTitel = "ND-Ackley2D";
        Assert.Equal(expectedTitel, actualTitel);
    }

    [Fact]
    public static void Constructor_ThrowsArgumentException_If_ArgumentDimenssion_IsEqualTo_Zero()
    {
        var action = () => new TestFunction(0, Vector1d, Vector1d, Vector1d);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("dimension == 0 (Parameter 'dimension')", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrowsArgumentException_If_ArgumentOptimumCount_IsNotEqualTo_ArgumnetDimension()
    {
        var action = () => new TestFunction(2, Vector1d, Vector1d, Vector1d);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("optimum.Count != dimension (Parameter 'optimum')", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrowsArgumentException_If_ArgumentLowerCount_IsNotEqualTo_ArgumnetDimension()
    {
        var action = () => new TestFunction(2, Vector2d00, Vector1d, Vector1d);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("lower.Count != dimension (Parameter 'lower')", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrowsArgumentException_If_ArgumentUpperCount_IsNotEqualTo_ArgumnetDimension()
    {
        var action = () => new TestFunction(2, Vector2d00, Vector2d00, Vector1d);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("upper.Count != dimension (Parameter 'upper')", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrowsArgumentException_If_ArgumentLower_IsNotAllLessThan_ArgumnetUpper()
    {
        var action = () => new TestFunction(2, Vector2d00, Vector2d01, Vector2d11);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("!lower.IsAllLessThan(upper)", exception.Message);
    }

    [Fact]
    public static void Constructor_ThrowsArgumentException_If_ArgumentOptimum_IsNotAllLessThan_ArgumnetUpper()
    {
        var action = () => new TestFunction(2, Vector2d00, Vector2d00, Vector2d11);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("!optimum.IsAllInInterval(lower, upper) (Parameter 'optimum')", exception.Message);
    }
}
