using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif

namespace OptimizationApplication.Extensions
{
    public static class StringExtensions
    {
        public static string[] SplitWithLengths(this string str, IntVector lengths)
        {
#if DEBUG
            if (str.Length != lengths.Sum())
            {
                throw new ArgumentException("The length of gived str is not equal to sum of lengths.");
            }
#endif
            int count = lengths.Count;
            string[] result = new string[count];
            int index = 0;

            for (int i = 0; i < count; i++)
            {
                result[i] = str.Substring(index, lengths[i]);
                index += lengths[i];
            }

            return result;
        }
    }
}
