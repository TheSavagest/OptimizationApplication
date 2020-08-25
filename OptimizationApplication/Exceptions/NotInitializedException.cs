using System;

namespace OptimizationApplication.Exceptions
{
    public sealed class NotInitializedException : Exception
    {
        public NotInitializedException(string name) : base($"The variable {name} is not initialized.")
        {
        }
    }
}
