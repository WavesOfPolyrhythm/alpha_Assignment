using System;
using System.Linq;
using System.Reflection;

public static class MapExtensions
{
    public static TDestination MapTo<TDestination>(this object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        TDestination destination = (TDestination)Activator.CreateInstance(typeof(TDestination))!;

        var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destinationProperty in destinationProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(x =>
                x.Name == destinationProperty.Name &&
                x.PropertyType == destinationProperty.PropertyType);

            if (sourceProperty != null && destinationProperty.CanWrite)
            {
                var value = sourceProperty.GetValue(source);
                destinationProperty.SetValue(destination, value);
            }
        }

        return destination;
    }
}
