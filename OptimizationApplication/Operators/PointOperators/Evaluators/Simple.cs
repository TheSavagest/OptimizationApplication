using OptimizationApplication.Solutions;
using OptimizationApplication.Vectors;
using static OptimizationApplication.Extensions.StructExtensions;

namespace OptimizationApplication.Operators.PointOperators.Evaluators
{
    public sealed class Simple<TSolution> : PointEvaluator<TSolution> where TSolution : Point
    {
        public Simple() : base()
        {
        }

        public override PointEvaluator<TSolution> Copy()
        {
            return new Simple<TSolution>
            {
                ValueFunction = ValueFunction
            };
        }

        public override void Evaluate(params TSolution[] population)
        {
            lock (this)
            {
                int count = population.Length;
                DoubleVector tmpCoordinates;

                for (int i = 0; i < count; i++)
                {
                    tmpCoordinates = StructOrException(population[i].Coordinates);
                    population[i].Value = ValueFunction(tmpCoordinates);
                }

                Evaluation += count;
            }
        }
    }
}
