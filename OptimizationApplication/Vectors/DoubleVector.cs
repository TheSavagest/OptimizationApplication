using System;
using static OptimizationApplication.Extensions.DoubleExtensions;
#if SIMD
using System.Numerics;
#endif

namespace OptimizationApplication.Vectors
{
    public struct DoubleVector
    {
        private readonly double[] Data;

        public readonly int Count => Data.Length;

        public readonly double First => Data[0];

        public readonly double Last => Data[^1];

        public double this[int index] => Data[index];

        private static readonly ArgumentOutOfRangeException CountLessThan1Exception = new ArgumentOutOfRangeException("count", "The argument count must be greater than 0.");

        private static readonly ArgumentException VectorsHaveDifferentCountException = new ArgumentException($"Vectors must have same counts.");

        private DoubleVector(double[] data)
        {
            Data = data;
        }

        public ReadOnlySpan<double> AsReadOnlySpan()
        {
            return new ReadOnlySpan<double>(Data);
        }

        public DoubleVector SubVector(int start, int count)
        {
#if DEBUG
            if (start < 0)
            {
                throw new ArgumentException($"The {nameof(start)} argument must be greater than or equal 0.");
            }
            if (Data.Length < start + count)
            {
                throw new ArgumentOutOfRangeException($"Can not copy from {nameof(start)} {nameof(count)} elements, because not enough elements.");
            }
#endif
            double[] oldData = Data;
            double[] newData = new double[count];

            Array.Copy(oldData, start, newData, 0, count);

            return new DoubleVector(newData);
        }

        public DoubleVector Ranks()
        {
            double[] data = Data;
            int length = data.Length;
            double[] ranks = new double[length];

            for (int i = 0; i < length; i++)
            {
                ranks[i] = 1.0 + CountLessThan(data[i]) + (CountEquals(data[i]) - 1.0) / 2.0;
            }

            return new DoubleVector(ranks);
        }

        public int CountLessThan(double value)
        {
            int i = 0;
            double[] data = Data;
            int count = 0;
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> valueVector = new Vector<double>(value);
            Vector<long> oneVector = Vector<long>.One;
            Vector<double> tmpVector;
            Vector<long> mask;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(data, i);
                mask = Vector.LessThan(tmpVector, valueVector);
                count -= (int)Vector.Dot(mask, oneVector);
            }
#endif
            for (; i < data.Length; i++)
            {
                if (data[i] < value)
                {
                    count++;
                }
            }

            return count;
        }

        public int CountEquals(double value)
        {
            int i = 0;
            double[] data = Data;
            int count = 0;
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> valueVector = new Vector<double>(value);
            Vector<long> oneVector = Vector<long>.One;
            Vector<double> tmpVector;
            Vector<long> mask;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(data, i);
                mask = Vector.Equals(tmpVector, valueVector);
                count -= (int)Vector.Dot(mask, oneVector);
            }
#endif
            for (; i < data.Length; i++)
            {
                if (IsEqualWithDelta(data[i], value))
                {
                    count++;
                }
            }

            return count;
        }

        public static double Distance(DoubleVector from, DoubleVector to)
        {
            return Math.Sqrt((from - to).Sqr().Sum());
        }

        #region Create
        public static class Create
        {
            public static DoubleVector Repeat(int count, double value)
            {
#if DEBUG
                if (count < 1)
                {
                    throw CountLessThan1Exception;
                }
#endif
                int i = 0;
                double[] newData = new double[count];
#if SIMD
                int vectorSize = Vector<double>.Count;
                Vector<double> valueVector = new Vector<double>(value);
                for (; i < newData.Length - vectorSize; i += vectorSize)
                {
                    valueVector.CopyTo(newData, i);
                }
#endif
                for (; i < newData.Length; i++)
                {
                    newData[i] = value;
                }

                return new DoubleVector(newData);
            }

            public static DoubleVector FromArray(double[] array)
            {
#if DEBUG
                if (array.Length < 1)
                {
                    throw new ArgumentException($"The length of array must be greater than 0.", nameof(array));
                }
#endif
                int length = array.Length;
                double[] newData = new double[length];

                Array.Copy(array, newData, length);

                return new DoubleVector(newData);
            }

            public static DoubleVector Sequence(double from, double step, int count)
            {
                //TODO SIMD
#if DEBUG
                if (count < 1)
                {
                    throw CountLessThan1Exception;
                }
#endif
                double[] newData = new double[count];

                for (int i = 0; i < newData.Length; i++)
                {
                    newData[i] = from + i * step;
                }

                return new DoubleVector(newData);
            }
        }
        #endregion

        #region Operators
        public static DoubleVector operator +(double value, DoubleVector vector)
        {
            int i = 0;
            double[] oldData = vector.Data;
            int length = oldData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> valueVector = new Vector<double>(value);
            Vector<double> tmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(oldData, i);
                (valueVector + tmpVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = value + oldData[i];
            }

            return new DoubleVector(newData);
        }

        public static DoubleVector operator +(DoubleVector left, DoubleVector right)
        {
#if DEBUG
            if (left.Count != right.Count)
            {
                throw VectorsHaveDifferentCountException;
            }
#endif
            int i = 0;
            double[] leftData = left.Data;
            double[] rightData = right.Data;
            int length = leftData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> leftTmpVector;
            Vector<double> rightTmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                leftTmpVector = new Vector<double>(leftData, i);
                rightTmpVector = new Vector<double>(rightData, i);
                (leftTmpVector + rightTmpVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = leftData[i] + rightData[i];
            }

            return new DoubleVector(newData);
        }

        public static DoubleVector operator -(DoubleVector vector, double value)
        {
            int i = 0;
            double[] oldData = vector.Data;
            int length = oldData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> valueVector = new Vector<double>(value);
            Vector<double> tmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(oldData, i);
                (tmpVector - valueVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = oldData[i] - value;
            }

            return new DoubleVector(newData);
        }

        public static DoubleVector operator -(DoubleVector left, DoubleVector right)
        {
#if DEBUG
            if (left.Count != right.Count)
            {
                throw VectorsHaveDifferentCountException;
            }
#endif
            int i = 0;
            double[] leftData = left.Data;
            double[] rightData = right.Data;
            int length = leftData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> leftTmpVector;
            Vector<double> rightTmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                leftTmpVector = new Vector<double>(leftData, i);
                rightTmpVector = new Vector<double>(rightData, i);
                (leftTmpVector - rightTmpVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = leftData[i] - rightData[i];
            }

            return new DoubleVector(newData);
        }

        public static DoubleVector operator *(double value, DoubleVector vector)
        {
            int i = 0;
            double[] oldData = vector.Data;
            int length = oldData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> valueVector = new Vector<double>(value);
            Vector<double> tmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(oldData, i);
                (valueVector * tmpVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = value * oldData[i];
            }

            return new DoubleVector(newData);
        }

        public static DoubleVector operator *(DoubleVector left, DoubleVector right)
        {
#if DEBUG
            if (left.Count != right.Count)
            {
                throw VectorsHaveDifferentCountException;
            }
#endif
            int i = 0;
            double[] leftData = left.Data;
            double[] rightData = right.Data;
            int length = leftData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> leftTmpVector;
            Vector<double> rightTmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                leftTmpVector = new Vector<double>(leftData, i);
                rightTmpVector = new Vector<double>(rightData, i);
                (leftTmpVector * rightTmpVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = leftData[i] * rightData[i];
            }

            return new DoubleVector(newData);
        }

        public static DoubleVector operator /(DoubleVector vector, double value)
        {
            int i = 0;
            double[] oldData = vector.Data;
            int length = oldData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> valueVector = new Vector<double>(value);
            Vector<double> tmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(oldData, i);
                (tmpVector / valueVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = oldData[i] / value;
            }

            return new DoubleVector(newData);
        }

        public static DoubleVector operator /(DoubleVector dividend, DoubleVector divider)
        {
#if DEBUG
            if (dividend.Count != divider.Count)
            {
                throw new ArgumentException($"Vectors must have same counts.");
            }
#endif
            int i = 0;
            double[] dividendData = dividend.Data;
            double[] dividerData = divider.Data;
            int length = dividendData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> dividendTmpVector;
            Vector<double> dividerTmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                dividendTmpVector = new Vector<double>(dividendData, i);
                dividerTmpVector = new Vector<double>(dividerData, i);
                (dividendTmpVector / dividerTmpVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = dividendData[i] / dividerData[i];
            }

            return new DoubleVector(newData);
        }
        #endregion

        #region Math
        public DoubleVector Cos()
        {
            //TODO SIMD
            double[] oldData = Data;
            int length = oldData.Length;
            double[] newData = new double[length];

            for (int i = 0; i < length; i++)
            {
                newData[i] = Math.Cos(oldData[i]);
            }

            return new DoubleVector(newData);
        }

        public DoubleVector Sin()
        {
            //TODO SIMD
            double[] oldData = Data;
            int length = oldData.Length;
            double[] newData = new double[length];

            for (int i = 0; i < length; i++)
            {
                newData[i] = Math.Sin(oldData[i]);
            }

            return new DoubleVector(newData);
        }

        public DoubleVector Sqr()
        {
            int i = 0;
            double[] oldData = Data;
            int length = oldData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> tmpVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(oldData, i);
                (tmpVector * tmpVector).CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = oldData[i] * oldData[i];
            }

            return new DoubleVector(newData);
        }

        public DoubleVector Sqrt()
        {
            int i = 0;
            double[] oldData = Data;
            int length = oldData.Length;
            double[] newData = new double[length];
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> tmpVector;
            Vector<double> sqrtVector;
            for (; i < length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(oldData, i);
                sqrtVector = Vector.SquareRoot(tmpVector);
                sqrtVector.CopyTo(newData, i);
            }
#endif
            for (; i < length; i++)
            {
                newData[i] = Math.Sqrt(oldData[i]);
            }

            return new DoubleVector(newData);
        }
        #endregion

        #region Statistics
        public double Sum()
        {
            int i = 0;
            double[] data = Data;
            double sum = 0.0;
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> accVector = Vector<double>.Zero;
            Vector<double> oneVector = Vector<double>.One;
            Vector<double> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(data, i);
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

        public double Max()
        {
            int i = 0;
            double[] data = Data;
            double max = double.NegativeInfinity;
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> maxVector = new Vector<double>(double.NegativeInfinity);
            Vector<double> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(data, i);
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

        public double Min()
        {
            int i = 0;
            double[] data = Data;
            double min = double.PositiveInfinity;
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> minVector = new Vector<double>(double.PositiveInfinity);
            Vector<double> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(data, i);
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

        public double Mul()
        {
            int i = 0;
            double[] data = Data;
            double mul = 1.0;
#if SIMD
            int vectorSize = Vector<double>.Count;
            Vector<double> accVector = Vector<double>.One;
            Vector<double> tmpVector;
            for (; i < data.Length - vectorSize; i += vectorSize)
            {
                tmpVector = new Vector<double>(data, i);
                accVector *= tmpVector;
            }
            for (int j = 0; j < vectorSize; j++)
            {
                mul *= accVector[j];
            }
#endif
            for (; i < data.Length; i++)
            {
                mul *= data[i];
            }

            return mul;
        }
        #endregion
    }
}

