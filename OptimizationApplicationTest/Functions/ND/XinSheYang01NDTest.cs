namespace OptimizationApplicationTest.Functions.ND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Functions.ND;
using System;
using Xunit;

public static class XinSheYang01NDNDTest
{
    [Fact]
    public static void XinSheYang01ND_CopyMethod_Returns_NotNull()
    {
        var function = new XinSheYang01ND(2);
        var copy = function.Copy();
        Assert.NotNull(copy);
    }

    [Fact]
    public static void XinSheYang01ND_GetValueMethod_ThrowsArgumentException_If_ArgumentCoordinatesCount_IsNotEqualTo_Dimension()
    {
        Action action = () => new XinSheYang01ND(2).GetValue(Vector<double>.Build.Dense(1));
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("coordinates.Count != Dimension (Parameter 'coordinates')", exception.Message);
    }

    [Fact]
    public static void XinSheYang01ND_GetValueMethod_ThrowsArgumentException_If_ArgumentCoordinates_IsNotAllInInterval_Of_LowerAndUpper()
    {
        Action action = () => new XinSheYang01ND(1).GetValue(Vector<double>.Build.Dense(1, double.MaxValue));
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("!coordinates.IsAllInInterval(Lower, Upper) (Parameter 'coordinates')", exception.Message);
    }

    [Theory]
    [InlineData(2, new double[] { 0.0, 0.0 }, 0.0)]
    [InlineData(2, new double[] { 0.4193113597, -0.7334554850 }, 0.42813172936144317)]
    public static void XinSheYang01ND_GetValueMethod_Returns_CorrectValue(
        byte dimension,
        double[] data,
        double expected)
    {
        var function = new XinSheYang01ND(dimension);
        var coordinates = Vector<double>.Build.Dense(data);
        var actual = function.GetValue(coordinates);
        Assert.Equal(expected, actual);
    }
}
