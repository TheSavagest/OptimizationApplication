using OptimizationApplication.Exceptions;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
using System.Text;

namespace OptimizationApplication.Operators.BinaryOperators.Crossovers
{
    public sealed class UniformCrossover<TSolution> : BinaryCrossover<TSolution> where TSolution : BinaryPoint, new()
    {
        public UniformCrossover() : base("U")
        {
        }

        public override BinaryCrossover<TSolution> Copy()
        {
            return new UniformCrossover<TSolution>();
        }

        public override TSolution[] Cross(TreadSafeRandom random, params TSolution[][] parents)
        {
#if DEBUG
            if (GenomeLength == null)
            {
                throw new NotInitializedException(nameof(GenomeLength));
            }
#endif
            int genomeLength = (int)GenomeLength;
            TSolution[] children = new TSolution[parents.Length];

            for (int c = 0; c < parents.Length; c++)
            {
                string firstParentGenome = parents[c][0].Genome ?? throw new NotInitializedException($"parents[{c}][0].Genome");
                string secondParentGenome = parents[c][1].Genome ?? throw new NotInitializedException($"parents[{c}][1].Genome");

                StringBuilder childGenome = new StringBuilder(genomeLength);

                for (int g = 0; g < genomeLength; g++)
                {
                    if (random.NextBool())
                    {
                        childGenome.Append(secondParentGenome[g]);
                    }
                    else
                    {
                        childGenome.Append(firstParentGenome[g]);
                    }
                }

                children[c] = new TSolution { Genome = childGenome.ToString() };
            }

            return children;
        }
    }
}
