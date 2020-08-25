using System;

namespace OptimizationApplication.Extensions
{
    public static class ClassExtensions
    {
        public static T ClassOrException<T>(T? value) where T : class
        {
            return value ?? throw new Exception();
        }
    }
}
