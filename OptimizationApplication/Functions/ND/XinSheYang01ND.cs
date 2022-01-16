namespace OptimizationApplication.Functions.ND
{
    using System;
    using MathNet.Numerics;
    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.Random;
    using OptimizationApplication.Extensions;

    // http://infinity77.net/global_optimization/test_functions_nd_X.html
    internal sealed class XinSheYang01ND : FunctionND
    {
        public override string Name => FunctionName.XinSheYang01;
        private readonly Vector<double> Range;
        private readonly IContinuousDistribution RandomnessDistribution;

        public XinSheYang01ND(
            byte dimension
        ) : base(
            dimension,
            Vector<double>.Build.Dense(dimension),
            Vector<double>.Build.Dense(dimension, -5),
            Vector<double>.Build.Dense(dimension, 5))
        {
            Range = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension));
            RandomnessDistribution = new ContinuousUniform(0.0, 1.0, new Mcg31m1(14111996, true));
        }

        public override Function Copy()
        {
            return new ZakharovND(Dimension);
        }

        public override double GetValue(
            Vector<double> coordinates)
        {
            if (coordinates.Count != Dimension)
                throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
            if (!coordinates.IsAllInInterval(Lower, Upper))
                throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));

            var random = Vector<double>.Build.Random(Dimension, RandomnessDistribution);

            return coordinates.PointwiseAbs().PointwisePower(Range).PointwiseMultiply(random).Sum();
        }
    }
}
