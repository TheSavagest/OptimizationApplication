using OptimizationApplication.Solutions;

namespace OptimizationApplication.Operators.BinaryOperators.Mutators
{
    public sealed class StrongMutator<TSolution> : BitFlipMutator<TSolution> where TSolution : BinaryPoint
    {
        public StrongMutator() : base("S", 3.0)
        {
        }
    }
}
