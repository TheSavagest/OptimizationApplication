namespace OptimizationApplication.Interfaces;

internal interface ICopyable<out TSelf>
{
    TSelf Copy();
}
