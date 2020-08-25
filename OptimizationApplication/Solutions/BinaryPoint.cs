namespace OptimizationApplication.Solutions
{
    public class BinaryPoint : Point
    {
        public string? Genome { get; set; }

        public BinaryPoint() : base()
        {
            Genome = null;
        }

        public BinaryPoint Copy()
        {
            return new BinaryPoint
            {
                Fitness = Fitness,
                Value = Value,
                Coordinates = Coordinates,
                Genome = new string(Genome)
            };
        }
    }
}
