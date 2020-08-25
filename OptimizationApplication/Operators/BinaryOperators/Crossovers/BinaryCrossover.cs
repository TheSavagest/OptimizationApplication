using OptimizationApplication.Functions;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
#if DEBUG
using System;
#endif

namespace OptimizationApplication.Operators.BinaryOperators.Crossovers
{
    public abstract class BinaryCrossover<TSolution> where TSolution : BinaryPoint, new()
    {
        public string Titel { get; }
        protected int? GenomeLength { get; private set; }

        protected BinaryCrossover(string titel)
        {
#if DEBUG
            if (string.IsNullOrEmpty(titel))
            {
                throw new ArgumentException("Given titel argument is null or empty.", nameof(titel));
            }
#endif
            Titel = titel;
            GenomeLength = null;
        }

        public abstract BinaryCrossover<TSolution> Copy();

        public void SetProblemData(Function function)
        {
            GenomeLength = function.GeneLengths.Sum();
        }

        public abstract TSolution[] Cross(TreadSafeRandom random, params TSolution[][] parents);
    }
}
