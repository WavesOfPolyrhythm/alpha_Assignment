using System;
using System.Linq;
using System.Reflection;

// NOTE: The MapTo method below was created with help from ChatGPT.
// Old MapTo only copied properties when source and destination had the same name and type.
// The new version:
// 1. Creates a new TDestination instance.
// 2. Copies all matching simple properties (name + type).
// 3. Detects related objects (properties with the same name but different types, e.g. ClientEntity → Client).
// 4. Maps those related objects by calling MapTo recursively.
// This ensures navigation properties like Client, Status, and User are populated instead of left null.


public static class MapExtensions
{
    public static TDestination MapTo<TDestination>(this object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var destinationType = typeof(TDestination);
        var destination = (TDestination)Activator.CreateInstance(destinationType)!;

        var sourceProps = source.GetType()
                                     .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destProps = destinationType
                                     .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destProp in destProps)
        {

            var srcProp = sourceProps.FirstOrDefault(p => p.Name == destProp.Name);
            if (srcProp == null || !destProp.CanWrite)
                continue;

            var srcValue = srcProp.GetValue(source);
            if (srcValue == null)
                continue;

            if (srcProp.PropertyType == destProp.PropertyType)
            {
                destProp.SetValue(destination, srcValue);
            }

            else if (!destProp.PropertyType.IsValueType
                     && destProp.PropertyType != typeof(string))
            {

                var mapMethod = typeof(MapExtensions)
                                    .GetMethod(nameof(MapTo), BindingFlags.Public | BindingFlags.Static)!
                                    .MakeGenericMethod(destProp.PropertyType);

                var nestedDest = mapMethod.Invoke(null, new object[] { srcValue });
                destProp.SetValue(destination, nestedDest);
            }
        }

        return destination;
    }
}
