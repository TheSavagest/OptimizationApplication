using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
#if DEBUG
using System;
#endif
using System.Linq;

namespace OptimizationApplication.Operators.BasicOperators.Selectors
{
    public abstract class BasicSelector<TSolution> where TSolution : Solution
    {
        public string Titel { get; }
        protected int ChildrenCount { get; }
        protected int ParentsCount { get; }
        protected bool IsDuplicatesPossible { get; }
        protected bool IsCoping { get; }

        protected BasicSelector(string titel, int childrenCount, int parentsCount, bool isDuplicatesEnabled, bool isCoping)
        {
#if DEBUG
            if (string.IsNullOrEmpty(titel))
            {
                throw new ArgumentException("Given titel argument is null or empty", nameof(titel));
            }
            if (childrenCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(childrenCount), childrenCount, $"The argument {nameof(childrenCount)} must be greater than 0.");
            }
            if (parentsCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(parentsCount), parentsCount, $"The argument {nameof(parentsCount)} must be greater than 0.");
            }
#endif
            Titel = titel;
            ChildrenCount = childrenCount;
            ParentsCount = parentsCount;
            IsDuplicatesPossible = isDuplicatesEnabled;
            IsCoping = isCoping;
        }

        public abstract BasicSelector<TSolution> Copy();

        protected abstract void BeforeSelection(TSolution[] population);

        protected abstract TSolution SelectOne(TreadSafeRandom random, TSolution[] population);

        public TSolution[][] Select(TreadSafeRandom random, TSolution[] population)
        {
            BeforeSelection(population);

            TSolution[][] parents = new TSolution[ChildrenCount][];

            for (int c = 0; c < parents.Length; c++)
            {
                parents[c] = new TSolution[ParentsCount];

                for (int p = 0; p < parents[c].Length; p++)
                {
                    TSolution selectedIndividual = SelectOne(random, population);

                    while (IsDuplicatesPossible && parents[c].Contains(selectedIndividual))
                    {
                        selectedIndividual = SelectOne(random, population);
                    }

                    if (IsCoping)
                    {
                        parents[c][p] = Solution.Copy(selectedIndividual);
                    }
                    else
                    {
                        parents[c][p] = selectedIndividual;
                    }
                }
            }

            return parents;
        }
    }
}
