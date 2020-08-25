using OptimizationApplication.Random;
using OptimizationApplication.Solutions;

namespace OptimizationApplication.Operators.BasicOperators.Selectors
{
    public sealed class Tournament<TSolution> : BasicSelector<TSolution> where TSolution : Solution
    {
        private int TournamentSize { get; }

        public Tournament(int childrenCount, int parentsCount, bool isDuplicatesEnabled, bool isCoping, int tournamentSize)
            : base($"T{tournamentSize}", childrenCount, parentsCount, isDuplicatesEnabled, isCoping)
        {
            TournamentSize = tournamentSize;
        }

        public override BasicSelector<TSolution> Copy()
        {
            return new Tournament<TSolution>(ChildrenCount, ParentsCount, IsDuplicatesPossible, IsCoping, TournamentSize);
        }

        protected override void BeforeSelection(TSolution[] population)
        {
        }

        protected override TSolution SelectOne(TreadSafeRandom random, TSolution[] population)
        {
            //It is not fully correct implementation, but usable
            int randomIndex = random.Next(population.Length);
            TSolution best = population[randomIndex];
            TSolution candidate;

            for (int i = 1; i < TournamentSize; i++)
            {
                randomIndex = random.Next(population.Length);
                candidate = population[randomIndex];

                if (candidate.IsBetterThan(best))
                {
                    best = candidate;
                }
            }

            return best;
        }
    }
}
