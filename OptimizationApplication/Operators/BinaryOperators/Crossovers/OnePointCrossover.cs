using OptimizationApplication.Exceptions;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
#if DEBUG
using System;
#endif
using System.Text;

namespace OptimizationApplication.Operators.BinaryOperators.Crossovers
{
    public sealed class OnePointCrossover<TSolution> : BinaryCrossover<TSolution> where TSolution : BinaryPoint, new()
    {
        public OnePointCrossover() : base("O")
        {
        }

        public override BinaryCrossover<TSolution> Copy()
        {
            return new OnePointCrossover<TSolution>();
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
                int point = random.Next(1, genomeLength - 1);
                StringBuilder childGenome = new StringBuilder(genomeLength);

                string firstParentGenome = parents[c][0].Genome ?? throw new NotInitializedException($"parents[{c}][0].Genome");
#if DEBUG
                if (firstParentGenome.Length != genomeLength)
                {
                    throw new ArgumentException($"parents[{c}][0].Genome has incorrect genome length.");
                }
#endif
                childGenome.Append(firstParentGenome[..point]);

                string secondParentGenome = parents[c][1].Genome ?? throw new NotInitializedException($"parents[{c}][1].Genome");
#if DEBUG
                if (secondParentGenome.Length != genomeLength)
                {
                    throw new ArgumentException($"parents[{c}][1].Genome has incorrect genome length.");
                }
#endif
                childGenome.Append(secondParentGenome[point..genomeLength]);

                children[c] = new TSolution { Genome = childGenome.ToString() };
            }

            return children;
        }
    }
}
