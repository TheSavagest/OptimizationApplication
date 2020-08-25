using OptimizationApplication.Solutions;

namespace OptimizationApplication.Operators.BinaryOperators.Mutators
{
    public sealed class MiddleMutator<TSolution> : BitFlipMutator<TSolution> where TSolution : BinaryPoint
    {
        public MiddleMutator() : base("M", 1.0)
        {
        }
    }
}
