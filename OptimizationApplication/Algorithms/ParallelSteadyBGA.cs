#if DEBUG
using OptimizationApplication.Exceptions;
#endif
using OptimizationApplication.Functions;
using OptimizationApplication.Operators.BasicOperators.Selectors;
using OptimizationApplication.Operators.BinaryOperators.Crossovers;
using OptimizationApplication.Operators.BinaryOperators.Decoders;
using OptimizationApplication.Operators.BinaryOperators.Initializers;
using OptimizationApplication.Operators.BinaryOperators.Mutators;
using OptimizationApplication.Operators.PointOperators.Evaluators;
using OptimizationApplication.Operators.PointOperators.Fitters;
using OptimizationApplication.Solutions;
using System.Threading.Tasks;

namespace OptimizationApplication.Algorithms
{
    public sealed class ParallelSteadyBGA<TSolution> : Algorithm<TSolution> where TSolution : BinaryPoint, new()
    {
        public override string Titel => $"ParallelSteadyBGA_{Selector.Titel}{Crossover.Titel}{Mutator.Titel}";

        protected override TSolution[]? Population { get; set; }
        protected override TSolution Best { get; set; }

        private BinaryInitializer<TSolution> Initializer { get; }
        private BinaryDecoder<TSolution> Decoder { get; }
        private PointFitter<TSolution> Fitter { get; }
        private BasicSelector<TSolution> Selector { get; }
        private BinaryCrossover<TSolution> Crossover { get; }
        private BinaryMutator<TSolution> Mutator { get; }

        public ParallelSteadyBGA(bool isCollectIterationStatistic, double accuracy, int maxIteration, int maxEvaluation,
            PointEvaluator<TSolution> evaluator, BinaryInitializer<TSolution> initializer, BinaryDecoder<TSolution> decoder,
            PointFitter<TSolution> fitter, BasicSelector<TSolution> selector, BinaryCrossover<TSolution> crossover,
            BinaryMutator<TSolution> mutator) : base(isCollectIterationStatistic, accuracy, maxIteration, maxEvaluation, evaluator)
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
        }

        public override Algorithm<TSolution> Copy()
        {
            return new ParallelSteadyBGA<TSolution>(IsCollectIterationStatistic, Accuracy, MaxIteration, MaxEvaluation, Evaluator,
                Initializer, Decoder, Fitter, Selector, Crossover, Mutator);
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
        }

        public override void Run()
        {
            Initialize();

            BeforeIteration();
#if DEBUG
            if (Population == null)
            {
                throw new NotInitializedException(nameof(Population));
            }
#endif
            int populationSize = Population.Length;

            Parallel.For(0, MaxEvaluation, i =>
            {
                if (!IsStopCondition())
                {
                    ProcessOne(i % populationSize);
                }
            });

            AfterIteration();
        }

        private void ProcessOne(int index)
        {
#if DEBUG
            if (Population == null)
            {
                throw new NotInitializedException(nameof(Population));
            }
            if (Random == null)
            {
                throw new NotInitializedException(nameof(Random));
            }
#endif
            var parents = new TSolution[2];
            lock (Population)
            {
                parents[0] = Solution.Copy(Population[index]);
                parents[1] = Selector.Select(Random, Population)[0][0];
            }

            var child = Crossover.Cross(Random, parents)[0];
            Mutator.Mutate(Random, child);
            Decoder.Decode(child);
            Evaluator.Evaluate(child);
            Fitter.Fit(child);

            lock (Population)
            {
                if (child.IsBetterThan(Population[index]))
                {
                    Population[index] = Solution.Copy(child);
                }
            }

            lock (Best)
            {
                UpdateBest(child);
            }
        }
    }
}
