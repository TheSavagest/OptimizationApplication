using System;
#if SIMD
using System.Numerics;
#endif

namespace OptimizationApplication.Vectors
{
    public struct LongVector
    {
        private readonly long[] Data;

        public readonly int Count => Data.Length;

        private LongVector(long[] data)
        {
            Data = data;
        }

        #region Create
        public static class Create
        {
            public static LongVector FromArray(long[] array)
            {
#if DEBUG
                if (array.Length < 1)
                {
                    throw new ArgumentException($"The length of array must be greater than 0.", nameof(array));
                }
#endif
                int length = array.Length;
                long[] newData = new long[length];

                Array.Copy(array, newData, length);

                return new LongVector(newData);
            }
        }
        #endregion

        #region Statistics
        public long Sum()
        {
            int i = 0;
            long[] data = Data;
            long sum = 0;
#if SIMD
            int vectorSize = Vector<long>.Count;
            Vector<long> accVector = Vector<long>.Zero;
            Vector<long> oneVector = Vector<long>.One;
            Vector<long> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<long>(data, i);
                accVector += tmpVector;
            }
            sum = Vector.Dot(accVector, oneVector);
#endif
            for (; i < data.Length; i++)
            {
                sum += data[i];
            }

            return sum;
        }

        public long Max()
        {
            int i = 0;
            long[] data = Data;
            long max = long.MinValue;
#if SIMD
            int vectorSize = Vector<long>.Count;
            Vector<long> maxVector = new Vector<long>(long.MinValue);
            Vector<long> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<long>(data, i);
                maxVector = Vector.Max(tmpVector, maxVector);
            }
            for (int j = 0; j < vectorSize; j++)
            {
                if (max < maxVector[j])
                {
                    max = maxVector[j];
                }
            }
#endif
            for (; i < data.Length; i++)
            {
                if (max < data[i])
                {
                    max = data[i];
                }
            }

            return max;
        }

        public long Min()
        {
            int i = 0;
            long[] data = Data;
            long min = long.MaxValue;
#if SIMD
            int vectorSize = Vector<long>.Count;
            Vector<long> minVector = new Vector<long>(long.MaxValue);
            Vector<long> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<long>(data, i);
                minVector = Vector.Min(tmpVector, minVector);
            }
            for (int j = 0; j < vectorSize; j++)
            {
                if (min > minVector[j])
                {
                    min = minVector[j];
                }
            }
#endif
            for (; i < data.Length; i++)
            {
                if (min > data[i])
                {
                    min = data[i];
                }
            }

            return min;
        }
        #endregion
    }
}
