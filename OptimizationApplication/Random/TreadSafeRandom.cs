#if DEBUG
using System;
#endif
//using static OptimizationApplication.Extensions.DoubleExtensions;
using OptimizationApplication.Vectors;
//#if SIMD
//using System.Numerics;
//#endif

namespace OptimizationApplication.Random
{
    public sealed class TreadSafeRandom : System.Random
    {
        private readonly object Lock = new object();

        public TreadSafeRandom() : base()
        {
        }

        public TreadSafeRandom(int randomSeed) : base(randomSeed)
        {
        }

        protected override double Sample()
        {
            lock (Lock)
            {
                return base.Sample();
            }
        }

        public bool NextBool()
        {
            return Sample() < 0.5;
        }

        public bool NextBoolWithProbability(double probability)
        {
#if DEBUG
            if (double.IsNaN(probability) || double.IsInfinity(probability))
            {
                throw new ArgumentException("Incorrect probability.", nameof(probability));
            }
            if (probability < 0.0 || probability > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(probability), probability, "Probability must be greater than or equal to 0.0 or less than or equal to 1.0.");
            }
#endif
            return Sample() < probability;
        }

        public int Roulette(DoubleVector probabilities)
        {
            int index = 0;
            int count = probabilities.Count;
            double value = probabilities.Sum() * Sample();
            //TODO
//#if SIMD
//            int vectorSize = Vector<double>.Count;
//            ReadOnlySpan<double> probSpan = probabilities.AsReadOnlySpan();
//            ReadOnlySpan<double> tmpSpan;
//            Vector<double> tmpVector;
//            Vector<double> oneVector = Vector<double>.One;
//            double tmpSum;
//            do
//            {
//                tmpSpan = probSpan.Slice(index, vectorSize);
//                tmpVector = new Vector<double>(tmpSpan);
//                tmpSum = Vector.Dot(tmpVector, oneVector);
//                value -= tmpSum;
//                index += vectorSize;
//            } while (value > 0.0 && index != count);

//            index -= vectorSize;
//            value += tmpSum;
//#endif
            do
            {
                value -= probabilities[index];
                index++;
            } while (value > 0.0 && index != count);

            index--;

            return index;
        }
    }
}
