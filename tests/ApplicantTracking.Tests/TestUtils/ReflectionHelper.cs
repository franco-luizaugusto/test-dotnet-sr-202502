using System;
using System.Reflection;

namespace ApplicantTracking.Tests.TestUtils;

internal static class ReflectionHelper
{
    public static void SetPrivateProperty<T>(object target, string propertyName, T value)
    {
        var prop = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (prop is null) throw new InvalidOperationException($"Property '{propertyName}' not found on type {target.GetType().FullName}.");
        prop.SetValue(target, value);
    }
}

