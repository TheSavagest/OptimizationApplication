using OptimizationApplication.Functions;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
#if DEBUG
using System;
#endif
using System.Text;
using static OptimizationApplication.Extensions.ClassExtensions;

namespace OptimizationApplication.Operators.BinaryOperators.Mutators
{
    public class BitFlipMutator<TSolution> : BinaryMutator<TSolution> where TSolution : BinaryPoint
    {
        protected BitFlipMutator(string titel, double mutationProbability) : base(titel, mutationProbability)
        {
#if DEBUG
            if (string.IsNullOrEmpty(titel))
            {
                throw new ArgumentException("Titel is null or empty.", nameof(titel));
            }
#endif
        }

        public override BinaryMutator<TSolution> Copy()
        {
            return new BitFlipMutator<TSolution>(Titel, MutationProbability);
        }

        public override void SetProblemData(Function function)
        {
            MutationProbability /= function.GeneLengths.Sum();
        }

        public override void Mutate(TreadSafeRandom random, params TSolution[] population)
        {
            StringBuilder tmpGenome;

            for (int i = 0; i < population.Length; i++)
            {
                tmpGenome = new StringBuilder(ClassOrException(population[i].Genome));

                for (int g = 0; g < tmpGenome.Length; g++)
                {
                    if (random.NextBoolWithProbability(MutationProbability))
                    {
                        if (tmpGenome[g] == '0')
                        {
                            tmpGenome[g] = '1';
                        }
                        else
                        {
                            tmpGenome[g] = '0';
                        }
                    }
                }

                population[i].Genome = tmpGenome.ToString();
            }
        }
    }
}
