namespace OptimizationApplication.Functions.ND
{
    using MathNet.Numerics;
    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.Random;

    // http://infinity77.net/global_optimization/test_functions_nd_X.html
    internal sealed class XinSheYang01ND : FunctionND
    {
        private readonly Vector<double> range;
        private readonly IContinuousDistribution randomnessDistribution;

        public XinSheYang01ND(
            byte dimension)
        : base(
            dimension,
            Vector<double>.Build.Dense(dimension),
            Vector<double>.Build.Dense(dimension, -5),
            Vector<double>.Build.Dense(dimension, 5))
        {
            range = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension));
            randomnessDistribution = new ContinuousUniform(0.0, 1.0, new Mcg31m1(14111996, true));
        }

        public override string Name => FunctionName.XinSheYang01;

        public override Function Copy()
        {
            return new XinSheYang01ND(Dimension);
        }

        public override double GetValue(Vector<double> coordinates)
        {
            CheckCoordinates(coordinates);

            var random = Vector<double>.Build.Random(Dimension, randomnessDistribution);

            return coordinates
                .PointwiseAbs()
                .PointwisePower(range)
                .PointwiseMultiply(random)
                .Sum();
        }
    }
}
