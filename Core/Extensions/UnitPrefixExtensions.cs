using PhysicalQuantities.Core.Enums;

namespace PhysicalQuantities.Core.Extensions;

public static class UnitPrefixExtensions
{
    public static double GetMultiplier(this UnitPrefix prefix)
    {
        return prefix switch
        {
            UnitPrefix.Nano => 0.000000001,    // 1e-9
            UnitPrefix.Micro => 0.000001,      // 1e-6
            UnitPrefix.Milli => 0.001,         // 1e-3
            UnitPrefix.Base => 1.0,            // 1e0
            UnitPrefix.Kilo => 1000.0,         // 1e3
            UnitPrefix.Mega => 1000000.0,      // 1e6
            UnitPrefix.Giga => 1000000000.0,   // 1e9
            _ => 1.0
        };
    }

    public static string GetSymbol(this UnitPrefix prefix)
    {
        return prefix switch
        {
            UnitPrefix.Nano => "n",
            UnitPrefix.Micro => "μ",
            UnitPrefix.Milli => "m",
            UnitPrefix.Base => "",
            UnitPrefix.Kilo => "k",
            UnitPrefix.Mega => "M",
            UnitPrefix.Giga => "G",
            _ => ""
        };
    }

    public static string GetName(this UnitPrefix prefix)
    {
        return prefix switch
        {
            UnitPrefix.Nano => "nano",
            UnitPrefix.Micro => "micro",
            UnitPrefix.Milli => "milli",
            UnitPrefix.Base => "",
            UnitPrefix.Kilo => "kilo",
            UnitPrefix.Mega => "mega",
            UnitPrefix.Giga => "giga",
            _ => ""
        };
    }
}