namespace OptimizationApplicationTest.Functions.ShiftableND;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Functions.ShiftableND;
using System;
using Xunit;

public static class AckleyTest
{
    [Fact]
    public static void Copy_Returns_CorrectValue()
    {
        var ackley = new Ackley(2, false);
        var copy = ackley.Copy();
        Assert.True(ackley.Equals(copy));
    }

    [Theory]
    [InlineData(2, 3)]
    public static void GetValue_ThrowsArgumentException_If_CoordinatesCount_IsNotEqualTo_FunctionDimension(
        byte functionDimension,
        byte coordinatesCount)
    {
        Func<object> action = () => new Ackley(functionDimension, false).GetValue(Vector<double>.Build.Dense(coordinatesCount));
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("coordinates.Count != Dimension (Parameter 'coordinates')", exception.Message);
    }

    [Theory]
    [InlineData(2, new double[] { -0.4193113597, 0.7334554850 }, new double[] { 0, 0 }, 4.35740908789858494288)]
    public static void GetValue_Returns_CorrectValue_If_FunctionShifted(
        byte dimension,
        double[] optimum,
        double[] input,
        double expected)
    {
        var ackley = new Ackley(dimension, false);
        var shiftedOptimum = Vector<double>.Build.Dense(optimum);
        const string optimumPropertyName = "Optimum";
        var ackleyType = ackley.GetType();
        ackleyType.GetProperty(optimumPropertyName)?.SetValue(ackley, shiftedOptimum, null);

        var coordinates = Vector<double>.Build.Dense(input);
        var actual = ackley.GetValue(coordinates);

        Assert.Equal(expected, actual);
    }
}
