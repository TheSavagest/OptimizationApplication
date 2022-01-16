using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using OptimizationApplication.Functions.ND;

var dimension = 2;
var range = Vector<double>.Build.Dense(Generate.LinearRange(1, dimension));
var randomnessDistribution = new ContinuousUniform(0.0, 1.0, new Mcg31m1(14111996, true));
var random = Vector<double>.Build.Random(dimension, randomnessDistribution);
var coordinates = Vector<double>.Build.Dense(new double[] { 0.4193113597, -0.7334554850 });
Console.WriteLine(coordinates.PointwiseAbs().PointwisePower(range).PointwiseMultiply(random).Sum());




//const byte dimension = 1;
//var function = new StyblinskiTangND(dimension);
//var opt = -39.16599 * dimension;
//var pv = -2.903533;
//var coordinates = Vector<double>.Build.Dense(dimension, -2.903534);
//var pf = function.GetValue(coordinates);
//Console.WriteLine(pf);
