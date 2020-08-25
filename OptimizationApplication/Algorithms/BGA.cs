#if DEBUG
using OptimizationApplication.Exceptions;
#endif
using OptimizationApplication.Functions;
using OptimizationApplication.Operators.BasicOperators.NewGenerationCreators;
using OptimizationApplication.Operators.BasicOperators.Selectors;
using OptimizationApplication.Operators.BinaryOperators.Crossovers;
using OptimizationApplication.Operators.BinaryOperators.Decoders;
using OptimizationApplication.Operators.BinaryOperators.Initializers;
using OptimizationApplication.Operators.BinaryOperators.Mutators;
using OptimizationApplication.Operators.PointOperators.Evaluators;
using OptimizationApplication.Operators.PointOperators.Fitters;
using OptimizationApplication.Solutions;

namespace OptimizationApplication.Algorithms
{
    public sealed class BGA<TSolution> : Algorithm<TSolution> where TSolution : BinaryPoint, new()
    {
        public override string Titel => $"BGA_{Selector.Titel}{Crossover.Titel}{Mutator.Titel}";

        protected override TSolution[]? Population { get; set; }
        protected override TSolution Best { get; set; }

        private BinaryInitializer<TSolution> Initializer { get; }
        private BinaryDecoder<TSolution> Decoder { get; }
        private PointFitter<TSolution> Fitter { get; }
        private BasicSelector<TSolution> Selector { get; }
        private BinaryCrossover<TSolution> Crossover { get; }
        private BinaryMutator<TSolution> Mutator { get; }
        private BasicNewGenerationCreator<TSolution> NewGenerationCreator { get; }

        public BGA(bool isCollectIterationStatistic, double accuracy, int maxIteration, int maxEvaluation,
            PointEvaluator<TSolution> evaluator, BinaryInitializer<TSolution> initializer, BinaryDecoder<TSolution> decoder,
            PointFitter<TSolution> fitter, BasicSelector<TSolution> selector, BinaryCrossover<TSolution> crossover,
            BinaryMutator<TSolution> mutator, BasicNewGenerationCreator<TSolution> newGenerationCreator) :
            base(isCollectIterationStatistic, accuracy, maxIteration, maxEvaluation, evaluator)
        {
            Population = null;
            Best = new TSolution
            {
                Fitness = double.MinValue
            };

            Initializer = initializer.Copy();
            Decoder = decoder.Copy();
            Fitter = fitter.Copy();
            Selector = selector.Copy();
            Crossover = crossover.Copy();
            Mutator = mutator.Copy();
            NewGenerationCreator = newGenerationCreator.Copy();
        }

        public override Algorithm<TSolution> Copy()
        {
            return new BGA<TSolution>(IsCollectIterationStatistic, Accuracy, MaxIteration, MaxEvaluation, Evaluator, Initializer, Decoder,
                Fitter, Selector, Crossover, Mutator, NewGenerationCreator);
        }

        public override void SetRunData(int randomSeed, int runIndex, Function function)
        {
            base.SetRunData(randomSeed, runIndex, function);

            Initializer.SetProblemData(function);
            Decoder.SetProblemData(function);
            Fitter.SetProblemData(function);
            Crossover.SetProblemData(function);
            Mutator.SetProblemData(function);
        }

        protected override void Initialize()
        {
#if DEBUG
            if (Random == null)
            {
                throw new NotInitializedException(nameof(Random));
            }
#endif
            BeforeInitialization();

            Population = Initializer.Initialize(Random);
            Decoder.Decode(Population);
            Evaluator.Evaluate(Population);
            Fitter.Fit(Population);
            UpdateBest(Population);

            AfterInitialization();
        }

        protected override void Iterate()
        {
#if DEBUG
            if (Random == null)
            {
                throw new NotInitializedException(nameof(Random));
            }
            if (Population == null)
            {
                throw new NotInitializedException(nameof(Population));
            }
#endif
            BeforeIteration();

            var parents = Selector.Select(Random, Population);
            var children = Crossover.Cross(Random, parents);
            Mutator.Mutate(Random, children);
            Decoder.Decode(children);
            Evaluator.Evaluate(children);
            Fitter.Fit(children);
            Population = NewGenerationCreator.Create(Population, children);
            UpdateBest(Population);

            AfterIteration();
        }
    }
}
