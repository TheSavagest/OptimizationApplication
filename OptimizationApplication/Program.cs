using OptimizationApplication.Algorithms;
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
using OptimizationApplication.Testing;
using System;
using System.IO;
using System.Linq;

namespace OptimizationApplication
{
    public static class Program
    {
        public static void Main()
        {
            string statisticPath = Path.Combine(@"C:\Stat", DateTime.Now.ToString("dd_MM_yyy_HH_mm"));
            Writer writer = new Writer(statisticPath);

            int runsNumber = 31;
            int randomSeed = 77883;
            //bool isParallelTesting = false;
            bool isParallelTesting = true;

            Tester<BinaryPoint> tester = new Tester<BinaryPoint>(writer, runsNumber, randomSeed, isParallelTesting);

            int dimension = 2;
            Algorithm<BinaryPoint>[] algorithms = CreateBGA(dimension).Concat(CreateParallelSteadyBGA(dimension)).ToArray();
            Function[] functions = CreateFunctions(dimension);

            tester.TestAll(algorithms, functions);

            Console.WriteLine("done, i hole all good =)");
            Console.ReadKey();
        }

        private static Algorithm<BinaryPoint>[] CreateBGA(int dimension)
        {
            bool collectIterationStatistic = false;
            double accuracy = 0.01;
            int populationSize = dimension * 100;
            int maxIteration = 100;
            int maxEvaluation = (maxIteration + 1) * populationSize;
            PointEvaluator<BinaryPoint> evaluator = new Simple<BinaryPoint>();
            BinaryDecoder<BinaryPoint> decoder = new BatchDecoder<BinaryPoint>();
            BinaryInitializer<BinaryPoint> initializer = new UniformInitializer<BinaryPoint>(populationSize);
            PointFitter<BinaryPoint> fitter = new NormalizingMinimization<BinaryPoint>();
            int childrenCount = populationSize;
            int parentsCount = 2;
            bool isDuplicatesEnabled = false;
            bool isCoping = true;
            int tournamentSize = dimension + 1;
            BasicSelector<BinaryPoint>[] selectors =
            {
                new Proportional<BinaryPoint>(childrenCount, parentsCount, isDuplicatesEnabled, isCoping),
                new Rank<BinaryPoint>(childrenCount, parentsCount, isDuplicatesEnabled, isCoping),
                new Tournament<BinaryPoint>(childrenCount, parentsCount, isDuplicatesEnabled, isCoping, tournamentSize)
            };
            BinaryCrossover<BinaryPoint>[] crossovers =
            {
                new OnePointCrossover<BinaryPoint>(),
                new TwoPointCrossover<BinaryPoint>(),
                new UniformCrossover<BinaryPoint>()
            };
            BinaryMutator<BinaryPoint>[] mutators =
            {
                new WeakMutator<BinaryPoint>(),
                new MiddleMutator<BinaryPoint>(),
                new StrongMutator<BinaryPoint>()
            };
            BasicNewGenerationCreator<BinaryPoint> newGenerationCreator = new ChildrenOnly<BinaryPoint>(populationSize, isCoping);

            Algorithm<BinaryPoint>[] algorithms = new Algorithm<BinaryPoint>[selectors.Length * crossovers.Length * mutators.Length];
            var ai = 0;
            foreach (BasicSelector<BinaryPoint> selector in selectors)
            {
                foreach (BinaryCrossover<BinaryPoint> crossover in crossovers)
                {
                    foreach (BinaryMutator<BinaryPoint> mutator in mutators)
                    {
                        algorithms[ai] = new BGA<BinaryPoint>(collectIterationStatistic, accuracy, maxIteration, maxEvaluation, evaluator,
                            initializer, decoder, fitter, selector, crossover, mutator, newGenerationCreator);

                        ai++;
                    }
                }
            }

            return algorithms;
        }

        private static Algorithm<BinaryPoint>[] CreateParallelSteadyBGA(int dimension)
        {
            bool collectIterationStatistic = false;
            double accuracy = 0.01;
            int populationSize = dimension * 100;
            int maxIteration = 100;
            int maxEvaluation = (maxIteration + 1) * populationSize;
            PointEvaluator<BinaryPoint> evaluator = new Simple<BinaryPoint>();
            BinaryDecoder<BinaryPoint> decoder = new BatchDecoder<BinaryPoint>();
            BinaryInitializer<BinaryPoint> initializer = new UniformInitializer<BinaryPoint>(populationSize);
            PointFitter<BinaryPoint> fitter = new NormalizingMinimization<BinaryPoint>();
            int childrenCount = 1;
            int parentsCount = 1;
            bool isDuplicatesEnabled = false;
            bool isCoping = true;
            int tournamentSize = dimension + 1;
            BasicSelector<BinaryPoint>[] selectors =
            {
                new Proportional<BinaryPoint>(childrenCount, parentsCount, isDuplicatesEnabled, isCoping),
                new Rank<BinaryPoint>(childrenCount, parentsCount, isDuplicatesEnabled, isCoping),
                new Tournament<BinaryPoint>(childrenCount, parentsCount, isDuplicatesEnabled, isCoping, tournamentSize)
            };
            BinaryCrossover<BinaryPoint>[] crossovers =
            {
                new OnePointCrossover<BinaryPoint>(),
                new TwoPointCrossover<BinaryPoint>(),
                new UniformCrossover<BinaryPoint>()
            };
            BinaryMutator<BinaryPoint>[] mutators =
            {
                new WeakMutator<BinaryPoint>(),
                new MiddleMutator<BinaryPoint>(),
                new StrongMutator<BinaryPoint>()
            };

            Algorithm<BinaryPoint>[] algorithms = new Algorithm<BinaryPoint>[selectors.Length * crossovers.Length * mutators.Length];
            var ai = 0;
            foreach (var selector in selectors)
            {
                foreach (BinaryCrossover<BinaryPoint> crossover in crossovers)
                {
                    foreach (BinaryMutator<BinaryPoint> mutator in mutators)
                    {
                        algorithms[ai] = new ParallelSteadyBGA<BinaryPoint>(collectIterationStatistic, accuracy, maxIteration, maxEvaluation,
                            evaluator, initializer, decoder, fitter, selector, crossover, mutator);

                        ai++;
                    }
                }
            }

            return algorithms;
        }

        private static Function[] CreateFunctions(int dimension)
        {
            Function[] functions =
            {
                new Ackley(dimension),
                new Griewank(dimension),
                new HappyCat(dimension),
                new Levy(dimension),
                new Rastrigin(dimension),
                new Rosenbrock(dimension),
                new Sphere(dimension),
                new StyblinskiTang(dimension),
                new SumSquares(dimension),
                new Zakharov(dimension)
            };

            return functions;
        }
    }
}
