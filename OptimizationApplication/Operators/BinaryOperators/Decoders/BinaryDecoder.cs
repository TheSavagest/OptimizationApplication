using OptimizationApplication.Functions;
using OptimizationApplication.Solutions;
using OptimizationApplication.Vectors;

namespace OptimizationApplication.Operators.BinaryOperators.Decoders
{
    public abstract class BinaryDecoder<TSolution> where TSolution : BinaryPoint
    {
        protected IntVector? GeneLengths { get; set; }
        protected DoubleVector? LowerSearchBorders { get; set; }
        protected DoubleVector? DiscretizationPeriod { get; set; }

        protected BinaryDecoder()
        {
            GeneLengths = null;
            LowerSearchBorders = null;
            DiscretizationPeriod = null;
        }

        public abstract BinaryDecoder<TSolution> Copy();

        public void SetProblemData(Function function)
        {
            GeneLengths = function.GeneLengths;
            LowerSearchBorders = function.LowerSearchBorders;
            DiscretizationPeriod = function.DiscretizationPeriod;
        }

        public abstract void Decode(params TSolution[] population);
    }
}
