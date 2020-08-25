using OptimizationApplication.Functions;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
#if DEBUG
using System;
#endif

namespace OptimizationApplication.Operators.BinaryOperators.Initializers
{
    public abstract class BinaryInitializer<TSolution> where TSolution : BinaryPoint, new()
    {
        protected int PopulationSize { get; }
        protected int? GenomeLength { get; set; }

        protected BinaryInitializer(int populationSize)
        {
#if DEBUG
            if (populationSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(populationSize), populationSize, "PopulationSize must be greater than 0");
            }
#endif
            PopulationSize = populationSize;
            GenomeLength = null;
        }

        public abstract BinaryInitializer<TSolution> Copy();

        public void SetProblemData(Function function)
        {
            GenomeLength = function.GeneLengths.Sum();
        }

        public abstract TSolution[] Initialize(TreadSafeRandom random);

        private static char RandomGene(TreadSafeRandom random, double probabilityOfOne)
        {
            return random.NextDouble() < probabilityOfOne ? '1' : '0';
        }

        protected static char[] RandomGenome(TreadSafeRandom random, double[] probabilitiesOfOne)
        {
            var genome = new char[probabilitiesOfOne.Length];

            for (var i = 0; i < genome.Length; i++)
            {
                genome[i] = RandomGene(random, probabilitiesOfOne[i]);
            }

            return genome;
        }
    }
}
