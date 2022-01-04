namespace OptimizationApplication.Functions;

using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;
using OptimizationApplication.Interfaces;
using static System.HashCode;
using static System.String;

public abstract class Function : ICopyable<Function>
{
    public string Titel => $"{Type}-{Name}{Dimension}D";
    public abstract string Name { get; }
    public abstract string Type { get; }
    public byte Dimension { get; }
    public Vector<double> Optimum { get; protected set; }
    public Vector<double> Lower { get; }
    public Vector<double> Upper { get; }

    protected Function(
        byte dimension,
        Vector<double> optimum,
        Vector<double> lower,
        Vector<double> upper)
    {
        if (dimension == 0)
            throw new ArgumentException("dimension == 0", nameof(dimension));
        if (optimum.Count != dimension)
            throw new ArgumentException("optimum.Count != dimension", nameof(optimum));
        if (lower.Count != dimension)
            throw new ArgumentException("lower.Count != dimension", nameof(lower));
        if (upper.Count != dimension)
            throw new ArgumentException("upper.Count != dimension", nameof(upper));

        Dimension = dimension;
        Optimum = optimum;
        Lower = lower;
        Upper = upper;
    }

    public override bool Equals(
        object? obj)
    {
        //Titel contains Type, Name and Dimension properties
        return this.IsEquals(
            obj,
            it => it.Titel,
            it => it.Optimum,
            it => it.Lower,
            it => it.Upper);
    }

    public override int GetHashCode()
    {
        return Combine(
            Titel,
            Optimum,
            Lower,
            Upper);
    }

    public override string ToString()
    {
        return $"{Titel} Optimum: [{Join(", ", Optimum)}] Lower: [{Join(", ", Lower)}] Upper: [{Join(", ", Upper)}]";
    }

    public abstract Function Copy();

    public abstract double GetValue(
        Vector<double> coordinates);
}
