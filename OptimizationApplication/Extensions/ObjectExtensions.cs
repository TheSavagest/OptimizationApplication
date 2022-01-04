namespace OptimizationApplication.Extensions;

public static class ObjectExtensions
{
    public static bool IsEquals<T>(
        this T it,
        object? obj,
        params Func<T, object?>[] getters)
    {
        if (obj is not T other)
            return false;

        foreach (var getter in getters)
        {
            var itValue = getter(it);
            var otherValue = getter(other);
            if (itValue is null && otherValue is null)
                continue;
            if (!itValue?.Equals(otherValue) ?? true)
                return false;
        }

        return true;
    }
}

