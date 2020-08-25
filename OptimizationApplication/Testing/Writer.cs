using OptimizationApplication.Statistics;
using System.IO;

namespace OptimizationApplication.Testing
{
    public sealed class Writer
    {
        private string StatisticPath { get; }

        public Writer(string statisticPath)
        {
            StatisticPath = statisticPath;
            Directory.CreateDirectory(StatisticPath);
        }

        public Writer Copy()
        {
            return new Writer(StatisticPath);
        }

        public void Write(string fileName, string header, object[] statistics)
        {
            string filePath = Path.Combine(StatisticPath, $@"{fileName}.csv");
            using StreamWriter outputFile = new StreamWriter(filePath);
            outputFile.WriteLine(header);
            foreach (var statistic in statistics)
            {
                outputFile.WriteLine(statistic);
            }
        }

        public void WriteAll(string header, AlgorithmStatistic[] statistics)
        {
            //SuccessRates
            string filePath = Path.Combine(StatisticPath, $@"!ALL_SR.csv");
            using StreamWriter outputFileSr = new StreamWriter(filePath);
            outputFileSr.WriteLine(header);
            for (int i = 0; i < statistics.Length; i++)
            {
                outputFileSr.WriteLine($"{statistics[i].AlgorithmName};{string.Join(';', statistics[i].SuccessRates)}");
            }

            //AverageEvaluations
            filePath = Path.Combine(StatisticPath, $@"!ALL_AE.csv");
            using StreamWriter outputFileAe = new StreamWriter(filePath);
            outputFileAe.WriteLine(header);
            for (int i = 0; i < statistics.Length; i++)
            {
                outputFileAe.WriteLine($"{statistics[i].AlgorithmName};{string.Join(';', statistics[i].AverageEvaluations)}");
            }

            //AverageTimeMs
            filePath = Path.Combine(StatisticPath, $@"!ALL_AT.csv");
            using StreamWriter outputFileAt = new StreamWriter(filePath);
            outputFileAt.WriteLine(header);
            for (int i = 0; i < statistics.Length; i++)
            {
                outputFileAt.WriteLine($"{statistics[i].AlgorithmName};{string.Join(';', statistics[i].AverageTimeMs)}");
            }
        }
    }
}
