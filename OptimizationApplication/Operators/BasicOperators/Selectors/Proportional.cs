using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
using OptimizationApplication.Vectors;
using System.Linq;
using static OptimizationApplication.Extensions.DoubleExtensions;
using static OptimizationApplication.Extensions.StructExtensions;
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Operators.BasicOperators.Selectors
{
    public sealed class Proportional<TSolution> : BasicSelector<TSolution> where TSolution : Solution
    {
        private DoubleVector SelectionProbabilities;

        public Proportional(int childrenCount, int parentsCount, bool isDuplicatesEnabled, bool isCoping) :
            base("P", childrenCount, parentsCount, isDuplicatesEnabled, isCoping)
        {
        }

        public override BasicSelector<TSolution> Copy()
        {
            return new Proportional<TSolution>(ChildrenCount, ParentsCount, IsDuplicatesPossible, IsCoping);
        }

        protected override void BeforeSelection(TSolution[] population)
        {
            double[] fitnesses = population.Select(individual => individual.Fitness)
                .Select(StructOrException)
                .ToArray();
            SelectionProbabilities = FromArray(fitnesses);
            double minFitness = SelectionProbabilities.Min();
            double maxFitness = SelectionProbabilities.Max();

            if (IsEqualWithDelta(minFitness, maxFitness))
            {
                int populationSize = population.Length;
                SelectionProbabilities = Repeat(populationSize, 1.0);
            }
        }

        protected override TSolution SelectOne(TreadSafeRandom random, TSolution[] population)
        {
            int randomIndex = random.Roulette(SelectionProbabilities);
            return population[randomIndex];
        }
    }
}
