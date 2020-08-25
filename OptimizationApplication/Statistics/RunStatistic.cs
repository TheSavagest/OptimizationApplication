namespace OptimizationApplication.Statistics
{
    public sealed class RunStatistic
    {
        public int? RunIndex { get; set; }
        public bool? IsSuccess { get; set; }
        public double? Distance { get; set; }
        public double? Value { get; set; }
        public int? Iteration { get; set; }
        public int? Evaluation { get; set; }
        public long? TimeMs { get; set; }

        public const string Header = "Run;IsSuccess;Distance;Value;Iteration;Evaluation;TimeMs";

        public RunStatistic()
        {
            RunIndex = null;
            IsSuccess = null;
            Distance = null;
            Value = null;
            Iteration = null;
            Evaluation = null;
            TimeMs = null;
        }

        public override string ToString()
        {
            return $"{RunIndex};{IsSuccess};{Distance};{Value};{Iteration};{Evaluation};{TimeMs}";
        }
    }
}
