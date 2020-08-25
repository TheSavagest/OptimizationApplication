using OptimizationApplication.Solutions;

namespace OptimizationApplication.Operators.BinaryOperators.Mutators
{
    public sealed class WeakMutator<TSolution> : BitFlipMutator<TSolution> where TSolution : BinaryPoint
    {
        public WeakMutator() : base("W", 1.0 / 3.0)
        {
        }
    }
}
