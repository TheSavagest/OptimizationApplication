using OptimizationApplication.Exceptions;
using OptimizationApplication.Functions;
using OptimizationApplication.Operators.PointOperators.Evaluators;
using OptimizationApplication.Random;
using OptimizationApplication.Solutions;
using OptimizationApplication.Statistics;
using OptimizationApplication.Vectors;
#if DEBUG
using System;
#endif
using System.Diagnostics;
using System.Linq;
using static OptimizationApplication.Extensions.StructExtensions;
using static OptimizationApplication.Vectors.DoubleVector;

namespace OptimizationApplication.Algorithms
{
    public abstract class Algorithm<TSolution> where TSolution : Point, new()
    {
        public abstract string Titel { get; }

        private int Iteration { get; set; }

        private Stopwatch Stopwatch { get; }
        public bool IsCollectIterationStatistic { get; }
        public IterationStatistic<TSolution>[]? IterationsStatistics { get; }
        public RunStatistic RunStatistic { get; }

        private DoubleVector? OptimalCoordinates { get; set; }
        protected double Accuracy { get; }
        protected int MaxIteration { get; }
        protected int MaxEvaluation { get; }

        protected abstract TSolution[]? Population { get; set; }
        protected abstract TSolution Best { get; set; }

        protected TreadSafeRandom? Random { get; private set; }

        protected PointEvaluator<TSolution> Evaluator { get; }

        protected Algorithm(bool isCollectIterationStatistic, double accuracy, int maxIteration, int maxEvaluation,
            PointEvaluator<TSolution> evaluator)
        {
#if DEBUG
            if (accuracy < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(accuracy), accuracy, "Accuracy must be greater than 0.0.");
            }
            if (maxIteration < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxIteration), maxIteration, "Max iteration must be greater than 0.");
            }
            if (maxEvaluation < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxEvaluation), maxEvaluation, "Max Evaluation must be greater than 0.");
            }
#endif
            Iteration = 0;

            Stopwatch = new Stopwatch();
            IsCollectIterationStatistic = isCollectIterationStatistic;
            IterationsStatistics = IsCollectIterationStatistic ? new IterationStatistic<TSolution>[maxIteration + 1] : null;
            RunStatistic = new RunStatistic();

            OptimalCoordinates = null;
            Accuracy = accuracy;
            MaxIteration = maxIteration;
            MaxEvaluation = maxEvaluation;

            Random = null;

            Evaluator = evaluator.Copy();
        }

        public abstract Algorithm<TSolution> Copy();

        public virtual void SetRunData(int randomSeed, int runIndex, Function function)
        {
            Random = new TreadSafeRandom(randomSeed);
            RunStatistic.RunIndex = runIndex;
            OptimalCoordinates = function.OptimalCoordinates;
            Evaluator.SetProblemData(function);
        }

        protected void BeforeInitialization()
        {
            Stopwatch.Start();
        }

        protected abstract void Initialize();

        protected void AfterInitialization()
        {
            Stopwatch.Stop();
            CollectIterationStatistic();
        }

        private void CollectIterationStatistic()
        {
            if (!IsCollectIterationStatistic)
            {
                return;
            }

            IterationStatistic<TSolution>[] iterationsStatistics = IterationsStatistics ?? throw new NotInitializedException(nameof(IterationsStatistics));
            TSolution[] population = Population ?? throw new NotInitializedException(nameof(Population));
            DoubleVector optimalCoordinates = OptimalCoordinates ?? throw new NotInitializedException(nameof(OptimalCoordinates));

            iterationsStatistics[Iteration] = new IterationStatistic<TSolution>(Iteration, population, Best, optimalCoordinates);
        }

        protected void UpdateBest(TSolution[] candidates)
        {
            TSolution bestCandidate = candidates.OrderBy(candidate => candidate.Fitness).Last();
            UpdateBest(bestCandidate);
        }

        protected void UpdateBest(TSolution candidate)
        {
            if (candidate.IsBetterThan(Best))
            {
                Best = Solution.Copy(candidate);

                DoubleVector bestCoordinates = StructOrException(Best.Coordinates);
                DoubleVector optimalCoordinates = StructOrException(OptimalCoordinates);
                RunStatistic.Distance = Distance(bestCoordinates, optimalCoordinates);
                RunStatistic.Value = Best.Value;
                RunStatistic.Iteration = Iteration;
                RunStatistic.Evaluation = Evaluator.Evaluation;
                RunStatistic.TimeMs = Stopwatch.ElapsedMilliseconds;
            }
        }

        protected bool IsStopCondition()
        {
            var isSuccess = RunStatistic.Distance < Accuracy;
            RunStatistic.IsSuccess = isSuccess;

            return isSuccess ||
                   Evaluator.Evaluation >= MaxEvaluation ||
                   Iteration >= MaxIteration;
        }

        protected void BeforeIteration()
        {
            Stopwatch.Start();
            Iteration++;
        }

        protected abstract void Iterate();

        protected void AfterIteration()
        {
            Stopwatch.Stop();
            CollectIterationStatistic();
        }

        public virtual void Run()
        {
            Initialize();

            while (!IsStopCondition())
            {
                Iterate();
            }
        }
    }
}
