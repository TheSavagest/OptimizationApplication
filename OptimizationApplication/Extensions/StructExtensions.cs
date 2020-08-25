using System;

namespace OptimizationApplication.Extensions
{
    public static class StructExtensions
    {
        public static T StructOrException<T>(T? value) where T : struct
        {
            return value ?? throw new Exception();
        }
    }
}
