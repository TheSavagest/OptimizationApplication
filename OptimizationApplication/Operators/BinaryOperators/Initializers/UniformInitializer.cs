#if DEBUG
using OptimizationApplication.Exceptions;
#endif
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;

namespace OptimizationApplication.Operators.BinaryOperators.Initializers
{
    public sealed class UniformInitializer<TSolution> : BinaryInitializer<TSolution> where TSolution : BinaryPoint, new()
    {
        public UniformInitializer(int populationSize) : base(populationSize)
        {
        }

        public override BinaryInitializer<TSolution> Copy()
        {
            return new UniformInitializer<TSolution>(PopulationSize)
            {
                GenomeLength = GenomeLength
            };
        }

        public override TSolution[] Initialize(TreadSafeRandom random)
        {
#if DEBUG
            if (GenomeLength == null)
            {
                throw new NotInitializedException(nameof(GenomeLength));
            }
#endif
            int genomeLength = (int)GenomeLength;
            char[] tmpGenome = new char[genomeLength];
            TSolution[] population = new TSolution[PopulationSize];

            for (int i = 0; i < population.Length; i++)
            {
                for (int g = 0; g < tmpGenome.Length; g++)
                {
                    if (random.NextBool())
                    {
                        tmpGenome[g] = '1';
                    }
                    else
                    {
                        tmpGenome[g] = '0';
                    }
                }

                population[i] = new TSolution { Genome = new string(tmpGenome) };
            }

            return population;
        }
    }
}
