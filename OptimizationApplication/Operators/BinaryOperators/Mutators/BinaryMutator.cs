using OptimizationApplication.Functions;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
#if DEBUG
using System;
#endif

namespace OptimizationApplication.Operators.BinaryOperators.Mutators
{
    public abstract class BinaryMutator<TSolution> where TSolution : BinaryPoint
    {
        public string Titel { get; }
        protected double MutationProbability { get; set; }

        protected BinaryMutator(string titel, double mutationProbability)
        {
#if DEBUG
            if (string.IsNullOrEmpty(titel))
            {
                throw new ArgumentException("Titel is null or empty.", nameof(titel));
            }
#endif
            Titel = titel;
            MutationProbability = mutationProbability;
        }

        public abstract BinaryMutator<TSolution> Copy();

        public abstract void SetProblemData(Function function);

        public abstract void Mutate(TreadSafeRandom random, params TSolution[] population);
    }
}
