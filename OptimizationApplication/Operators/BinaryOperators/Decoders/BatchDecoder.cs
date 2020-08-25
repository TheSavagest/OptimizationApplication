#if DEBUG
using OptimizationApplication.Exceptions;
#endif
using OptimizationApplication.Extensions;
using OptimizationApplication.Solutions;
using OptimizationApplication.Vectors;
using System;
using System.Linq;
using static OptimizationApplication.Extensions.ClassExtensions;
using static OptimizationApplication.Vectors.DoubleVector.Create;

namespace OptimizationApplication.Operators.BinaryOperators.Decoders
{
    public sealed class BatchDecoder<TSolution> : BinaryDecoder<TSolution> where TSolution : BinaryPoint
    {
        public BatchDecoder() : base()
        {
        }

        public override BinaryDecoder<TSolution> Copy()
        {
            return new BatchDecoder<TSolution>
            {
                GeneLengths = GeneLengths,
                LowerSearchBorders = LowerSearchBorders,
                DiscretizationPeriod = DiscretizationPeriod
            };
        }

        public override void Decode(params TSolution[] population)
        {
#if DEBUG
            if (GeneLengths == null)
            {
                throw new NotInitializedException(nameof(GeneLengths));
            }
#endif
            IntVector geneLengths = (IntVector)GeneLengths;
            string tmpGenome;
            string[] tmpGenes;
            DoubleVector tmpNodes;

            for (int i = 0; i < population.Length; i++)
            {
                tmpGenome = ClassOrException(population[i].Genome);
                tmpGenes = tmpGenome.SplitWithLengths(geneLengths);
                tmpNodes = FromArray(tmpGenes.Select(gene => (double)Convert.ToInt32(string.Join("", gene), 2))
                                             .ToArray());
                population[i].Coordinates = tmpNodes * DiscretizationPeriod + LowerSearchBorders;
            }
        }
    }
}
