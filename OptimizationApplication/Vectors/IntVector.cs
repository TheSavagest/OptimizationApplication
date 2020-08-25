using System;
#if SIMD
using System.Numerics;
#endif

namespace OptimizationApplication.Vectors
{
    public struct IntVector
    {
        private readonly int[] Data;

        public readonly int Count => Data.Length;

        public int this[int index] => Data[index];

        private IntVector(int[] data)
        {
            Data = data;
        }

        #region Create
        public static class Create
        {
            public static IntVector Repeat(int count, int value)
            {
#if DEBUG
                if (count < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(count), count, $"The argument {nameof(count)} must be greater than 0.");
                }
#endif
                int i = 0;
                int[] newData = new int[count];
#if SIMD
                int vectorSize = Vector<int>.Count;
                Vector<int> valueVector = new Vector<int>(value);
                for (i = 0; i < newData.Length - vectorSize; i += vectorSize)
                {
                    valueVector.CopyTo(newData, i);
                }
#endif
                for (; i < newData.Length; i++)
                {
                    newData[i] = value;
                }

                return new IntVector(newData);
            }

            public static IntVector FromArray(int[] array)
            {
#if DEBUG
                if (array.Length < 1)
                {
                    throw new ArgumentException($"The length of array must be greater than 0.", nameof(array));
                }
#endif
                int length = array.Length;
                int[] newData = new int[length];

                Array.Copy(array, newData, length);

                return new IntVector(newData);
            }
        }
        #endregion

        #region Statistics
        public int Sum()
        {
            int i = 0;
            int[] data = Data;
            int sum = 0;
#if SIMD
            int vectorSize = Vector<int>.Count;
            Vector<int> accVector = Vector<int>.Zero;
            Vector<int> oneVector = Vector<int>.One;
            Vector<int> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<int>(data, i);
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

        public int Max()
        {
            int i = 0;
            int[] data = Data;
            int max = int.MinValue;
#if SIMD
            int vectorSize = Vector<int>.Count;
            Vector<int> maxVector = new Vector<int>(int.MinValue);
            Vector<int> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<int>(data, i);
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

        public int Min()
        {
            int i = 0;
            int[] data = Data;
            int min = int.MaxValue;
#if SIMD
            int vectorSize = Vector<int>.Count;
            Vector<int> minVector = new Vector<int>(int.MaxValue);
            Vector<int> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<int>(data, i);
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
