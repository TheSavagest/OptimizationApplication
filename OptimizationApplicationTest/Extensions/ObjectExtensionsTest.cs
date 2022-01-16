//namespace OptimizationApplicationTest.Extensions;

//using OptimizationApplication.Extensions;
//using Xunit;

//internal sealed class TestObject<T>
//{
//    internal T? Property { get; set; }
//}

//public class ObjectExtensionsTest
//{
//    [Fact]
//    public static void IsEqual_Returns_False_If_ArgumentsDifferentType()
//    {
//        var actual = 0.IsEquals(0.0);
//        Assert.False(actual);
//    }

//    [Fact]
//    public static void IsEqual_Returns_True_If_BothProperties_Are_Null()
//    {
//        var obj1 = new TestObject<string>() { Property = null };
//        var obj2 = new TestObject<string>() { Property = null };
//        var actual = obj1.IsEquals(obj2, it => it.Property);
//        Assert.True(actual);
//    }

//    [Fact]
//    public static void IsEqual_Returns_False_If_FirstArgumentProperty_Is_Null()
//    {
//        var obj1 = new TestObject<string>() { Property = null };
//        var obj2 = new TestObject<string>() { Property = string.Empty };
//        var actual = obj1.IsEquals(obj2, it => it.Property);
//        Assert.False(actual);
//    }
//}
