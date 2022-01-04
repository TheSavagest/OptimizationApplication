namespace OptimizationApplicationTest.Extensions;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using OptimizationApplication.Extensions;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

public static class VectorDoubleBuilderExtensionsTest
{
    [Fact]
    public static void VectorDoubleBuilderExtensions_RandomSource_Is_ThreadSafe()
    {
        var type = typeof(VectorDoubleBuilderExtensions);
        var info = type.GetField("randomSource", BindingFlags.NonPublic | BindingFlags.Static);
        var randomSource = info?.GetValue(null) as RandomSource;

        type = typeof(RandomSource);
        info = type.GetField("_threadSafe", BindingFlags.NonPublic | BindingFlags.Instance);
        var isThreadSafe = info?.GetValue(randomSource) as bool?;

        Assert.True(isThreadSafe);
    }

    [Theory]
    [InlineData(10)]
    public static void UniformMethed_Returns_VectorDouble_With_CorrectCount(
        int count)
    {
        var actural = Vector<double>.Build.Uniform(count).Count;
        Assert.Equal(count, actural);
    }

    [Theory]
    [InlineData(10)]
    public static void UniformMethed_Returns_NotZerosVectorDouble(
        int count)
    {
        var isAllNotZero = Vector<double>.Build.Uniform(count).All(it => it != 0.0);
        Assert.True(isAllNotZero);
    }

    [Theory]
    [InlineData(10, 0.0, 0.0)]
    [InlineData(10, 1.0, 0.0)]
    public static void RandomMethed_ThrowsArgumentException_If_MinArgument_IsGreaterThanOrEqualTo_MaxArgument(
        int length,
        double min,
        double max)
    {
        var action = () => Vector<double>.Build.Random(length, min, max);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("max <= min", exception.Message);
    }

    // if this test failed then randomSource.Seed was changed
    [Theory]
    [InlineData(10, 20)]
    public static void RandomMethed_Returns_VectorDouble_With_CorrectRange(
        int length,
        double range)
    {
        var min = range / -2;
        var max = range / 2;
        var vector = Vector<double>.Build.Random(length, min, max);
        var actual = (vector.Maximum() - vector.Minimum()) >= max;
        Assert.True(actual);
    }
}
