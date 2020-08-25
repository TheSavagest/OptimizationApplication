using OptimizationApplication.Solutions;

namespace OptimizationApplication.Operators.BasicOperators.NewGenerationCreators
{
    public abstract class BasicNewGenerationCreator<TSolution> where TSolution : Solution
    {
        protected int NewPopulationSize { get; }
        protected bool IsCoping { get; }

        protected BasicNewGenerationCreator(int newPopulationSize, bool isCoping)
        {
            NewPopulationSize = newPopulationSize;
            IsCoping = isCoping;
        }

        public abstract BasicNewGenerationCreator<TSolution> Copy();

        public abstract TSolution[] Create(TSolution[] population, TSolution[] children);
    }
}
