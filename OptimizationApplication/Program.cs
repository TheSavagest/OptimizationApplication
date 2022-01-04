using System.Runtime.CompilerServices;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using OptimizationApplication.Extensions;
using OptimizationApplication.Functions.ShiftableND;

[assembly: InternalsVisibleTo("OptimizationApplicationTest")]

Control.UseBestProviders();

Console.WriteLine(Vector<double>.Build.Random(100, -10, 10));


