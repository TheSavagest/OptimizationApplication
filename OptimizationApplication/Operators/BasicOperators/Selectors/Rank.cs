using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
using OptimizationApplication.Vectors;
using System.Linq;
using static OptimizationApplication.Extensions.StructExtensions;
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Operators.BasicOperators.Selectors
{
    public sealed class Rank<TSolution> : BasicSelector<TSolution> where TSolution : Solution
    {
        private DoubleVector SelectionProbabilities;

        public Rank(int childrenCount, int parentsCount, bool isDuplicatesEnabled, bool isCoping) :
            base("R", childrenCount, parentsCount, isDuplicatesEnabled, isCoping)
        {
        }

        public override BasicSelector<TSolution> Copy()
        {
            return new Rank<TSolution>(ChildrenCount, ParentsCount, IsDuplicatesPossible, IsCoping);
        }

        protected override void BeforeSelection(TSolution[] population)
        {
            double[] fitnesses = population.Select(individual => individual.Fitness)
                .Select(StructOrException)
                .ToArray();
            SelectionProbabilities = FromArray(fitnesses).Ranks();
        }

        protected override TSolution SelectOne(TreadSafeRandom random, TSolution[] population)
        {
            int randomIndex = random.Roulette(SelectionProbabilities);
            return population[randomIndex];
        }
    }
}
