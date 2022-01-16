namespace OptimizationApplicationTest.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Functions.ND;
using System;
using Xunit;

public static class StyblinskiTangNDTest
{
    [Fact]
    public static void StyblinskiTangND_CopyMethod_Returns_NotNull()
    {
        var function = new StyblinskiTangND(2);
        var copy = function.Copy();
        Assert.NotNull(copy);
    }

    [Theory]
    [InlineData(2, new double[] { -2.90353401818596, -2.90353401818596 })]
    public static void StyblinskiTangND_HasCorrect_Optimum(byte dimension, double[] expectedData)
    {
        var function = new StyblinskiTangND(dimension);
        var actual = function.Optimum;
        var expected = Vector<double>.Build.Dense(expectedData);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void StyblinskiTangND_GetValueMethod_ThrowsArgumentException_If_ArgumentCoordinatesCount_IsNotEqualTo_Dimension()
    {
        Action action = () => new StyblinskiTangND(2).GetValue(Vector<double>.Build.Dense(1));
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("coordinates.Count != Dimension (Parameter 'coordinates')", exception.Message);
    }

    [Fact]
    public static void StyblinskiTangND_GetValueMethod_ThrowsArgumentException_If_ArgumentCoordinates_IsNotAllInInterval_Of_LowerAndUpper()
    {
        Action action = () => new StyblinskiTangND(1).GetValue(Vector<double>.Build.Dense(1, double.MaxValue));
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("!coordinates.IsAllInInterval(Lower, Upper) (Parameter 'coordinates')", exception.Message);
    }

    [Theory]
    [InlineData(2, new double[] { 0.4193113597, -0.7334554850 }, -6.335436502122449908824819431175)]
    public static void StyblinskiTangND_GetValueMethod_Returns_CorrectValue(
        byte dimension,
        double[] data,
        double expected)
    {
        var function = new StyblinskiTangND(dimension);
        var coordinates = Vector<double>.Build.Dense(data);
        var actual = function.GetValue(coordinates);
        Assert.Equal(expected, actual);
    }
}
