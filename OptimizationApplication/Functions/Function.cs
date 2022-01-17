namespace OptimizationApplication.Functions;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;
using OptimizationApplication.Interfaces;

public abstract class Function : ITiteled, ICopyable<Function>
{
    protected Function(
        byte dimension,
        Vector<double> optimum,
        Vector<double> lower,
        Vector<double> upper)
    {
        if (dimension == 0)
        {
            throw new ArgumentException("dimension == 0", nameof(dimension));
        }

        if (optimum.Count != dimension)
        {
            throw new ArgumentException("optimum.Count != dimension", nameof(optimum));
        }

        if (lower.Count != dimension)
        {
            throw new ArgumentException("lower.Count != dimension", nameof(lower));
        }

        if (upper.Count != dimension)
        {
            throw new ArgumentException("upper.Count != dimension", nameof(upper));
        }

        if (!lower.IsAllLessThan(upper))
        {
            throw new ArgumentException("!lower.IsAllLessThan(upper)");
        }

        if (!optimum.IsAllInInterval(lower, upper))
        {
            throw new ArgumentException("!optimum.IsAllInInterval(lower, upper)", nameof(optimum));
        }

        Dimension = dimension;
        Optimum = optimum;
        Lower = lower;
        Upper = upper;
    }

    public abstract string Type { get; }

    public abstract string Name { get; }

    public string Titel => $"{Type}-{Name}{Dimension}D";

    public byte Dimension { get; }

    public Vector<double> Optimum { get; }

    public Vector<double> Lower { get; }

    public Vector<double> Upper { get; }

    public abstract Function Copy();

    public abstract double GetValue(Vector<double> coordinates);

    protected void CheckCoordinates(
        Vector<double> coordinates)
    {
        if (coordinates.Count != Dimension)
        {
            throw new ArgumentException("coordinates.Count != Dimension", nameof(coordinates));
        }

        if (!coordinates.IsAllInInterval(Lower, Upper))
        {
            throw new ArgumentException("!coordinates.IsAllInInterval(Lower, Upper)", nameof(coordinates));
        }
    }
}
