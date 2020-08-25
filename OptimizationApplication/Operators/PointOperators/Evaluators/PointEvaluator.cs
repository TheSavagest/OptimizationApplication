using OptimizationApplication.Functions;
using OptimizationApplication.Solutions;
using OptimizationApplication.Vectors;
using System;

namespace OptimizationApplication.Operators.PointOperators.Evaluators
{
    public abstract class PointEvaluator<TSolution> where TSolution : Point
    {
        protected Func<DoubleVector, double> ValueFunction { get; set; }
        public int Evaluation { get; protected set; }

        protected PointEvaluator()
        {
            ValueFunction = coordinates => throw new Exception();
            Evaluation = 0;
        }

        public abstract PointEvaluator<TSolution> Copy();

        public void SetProblemData(Function function)
        {
            ValueFunction = function.ValueFunction;
        }

        public abstract void Evaluate(params TSolution[] population);
    }
}
