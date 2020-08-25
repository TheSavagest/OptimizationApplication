using OptimizationApplication.Functions;
using OptimizationApplication.Solutions;

namespace OptimizationApplication.Operators.PointOperators.Fitters
{
    public abstract class PointFitter<TSolution> where TSolution : Point
    {
        protected bool? IsMinimization { get; set; }

        public PointFitter()
        {
            IsMinimization = null;
        }

        public abstract PointFitter<TSolution> Copy();

        public void SetProblemData(Function function)
        {
            IsMinimization = function.IsMinimization;
        }

        public abstract void Fit(params TSolution[] population);
    }
}
