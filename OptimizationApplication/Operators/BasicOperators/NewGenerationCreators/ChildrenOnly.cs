using OptimizationApplication.Solutions;
#if DEBUG
using System;
#endif
using System.Collections.Generic;
using System.Linq;

namespace OptimizationApplication.Operators.BasicOperators.NewGenerationCreators
{
    public sealed class ChildrenOnly<TSolution> : BasicNewGenerationCreator<TSolution> where TSolution : Solution
    {
        public ChildrenOnly(int newPopulationSize, bool isCoping) : base(newPopulationSize, isCoping)
        {
#if DEBUG
            if (newPopulationSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(newPopulationSize), newPopulationSize, $"The argument {nameof(newPopulationSize)} must be greater than 0.");
            }
#endif
        }

        public override BasicNewGenerationCreator<TSolution> Copy()
        {
            return new ChildrenOnly<TSolution>(NewPopulationSize, IsCoping);
        }

        public override TSolution[] Create(TSolution[] population, TSolution[] children)
        {
#if DEBUG
            if (population.Length + children.Length < NewPopulationSize)
            {
                throw new ArgumentException("Given population and children contain not enough individuals to create new generation.");
            }
#endif
            IOrderedEnumerable<TSolution> oldPopulation = population.OrderBy(individual => individual.Fitness);
            IOrderedEnumerable<TSolution> newPopulation = children.OrderBy(individual => individual.Fitness);

            IEnumerable<TSolution> tmpPopulation = newPopulation.Concat(oldPopulation).Take(NewPopulationSize);

            if (IsCoping)
            {
                tmpPopulation = tmpPopulation.Select(Solution.Copy);
            }

            return tmpPopulation.ToArray();
        }
    }
}
