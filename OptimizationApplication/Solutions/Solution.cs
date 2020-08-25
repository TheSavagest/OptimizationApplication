#if DEBUG 
using OptimizationApplication.Exceptions;
#endif
using System;

namespace OptimizationApplication.Solutions
{
    public abstract class Solution
    {
        public double? Fitness { get; set; }

        protected Solution()
        {
            Fitness = null;
        }

        public bool IsBetterThan(Solution other)
        {
#if DEBUG
            if (Fitness == null)
            {
                throw new NotInitializedException(nameof(Fitness));
            }
            if (other.Fitness == null)
            {
                throw new NotInitializedException(nameof(other.Fitness));
            }
#endif
            return Fitness > other.Fitness;
        }

        public static T Copy<T>(T solution) where T : Solution
        {
            if (solution is BinaryPoint binaryPoint)
            {
                return binaryPoint.Copy() as T ?? throw new InvalidCastException();
            }

            throw new ArgumentException($"Unknown type of {nameof(solution)}.");
        }
    }
}
