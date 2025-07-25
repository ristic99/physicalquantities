using System.Globalization;
using System.Windows.Data;
using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;

namespace PhysicalQuantities.Core.Converters
{
    /// <summary>
    /// Simple converter that works with struct PhysicalQuantity - clean and fast
    /// </summary>
    public class PhysicalQuantityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Handle nullable struct
            if (value is not PhysicalQuantity quantity)
                return string.Empty;

            // If parameter is provided, try to convert to that prefix
            if (parameter != null)
            {
                UnitPrefix targetPrefix;

                // Handle different parameter types
                if (parameter is UnitPrefix prefix)
                {
                    targetPrefix = prefix;
                }
                else if (parameter is string stringParam && Enum.TryParse(stringParam, out UnitPrefix parsedPrefix))
                {
                    targetPrefix = parsedPrefix;
                }
                else
                {
                    // Invalid parameter, fall back to base units display
                    return quantity.ToString();
                }

                return quantity.ToString(targetPrefix);
            }

            // No parameter provided, use base units
            return quantity.ToString();
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Default to Voltage type if no parameter specified
            if (value is not string stringValue || !double.TryParse(stringValue, out var numericValue))
            {
                return new PhysicalQuantity(0, PhysicalQuantityType.Voltage);
            }

            var type = PhysicalQuantityType.Voltage; // Default

            // Use parameter to specify the type
            if (parameter is string typeParam)
            {
                Enum.TryParse(typeParam, out type);
            }

            return new PhysicalQuantity(numericValue, type);
        }
    }

    /// <summary>
    /// Simple converter that only shows the numeric value in the specified prefix
    /// </summary>
    public class PhysicalQuantityValueConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not PhysicalQuantity quantity)
                return string.Empty;

            // If parameter is provided, convert to that prefix and show only the value
            if (parameter != null)
            {
                UnitPrefix targetPrefix;

                if (parameter is UnitPrefix prefix)
                {
                    targetPrefix = prefix;
                }
                else if (parameter is string stringParam && Enum.TryParse(stringParam, out UnitPrefix parsedPrefix))
                {
                    targetPrefix = parsedPrefix;
                }
                else
                {
                    return quantity.GetValueIn(UnitPrefix.Base).ToString("G6");
                }

                // Get just the numeric value in the specified prefix
                var valueInPrefix = quantity.GetValueIn(targetPrefix);
                return valueInPrefix.ToString("G6");
            }

            return quantity.GetValueIn(UnitPrefix.Base).ToString("G6");
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException("PhysicalQuantityValueConverter does not support ConvertBack");
        }
    }
}