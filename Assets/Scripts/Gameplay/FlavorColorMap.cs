using UnityEngine;
using System.Collections.Generic;

public static class FlavorColorMap
{
    private static Dictionary<FlavorType, Color> map;

    static FlavorColorMap()
    {
        map = new Dictionary<FlavorType, Color>();
        // Example; set actual colors to your taste
        map[FlavorType.Vanilla] = new Color(1f, 0.95f, 0.8f);
        map[FlavorType.Chocolate] = new Color(0.35f, 0.18f, 0.07f);
        map[FlavorType.Strawberry] = new Color(1f, 0.45f, 0.6f);
        map[FlavorType.Mint] = new Color(0.6f, 1f, 0.85f);
        // Add others or modify as necessary
    }

    public static Color GetColorForFlavor(FlavorType f)
    {
        if (map.ContainsKey(f)) return map[f];
        return Color.white;
    }
}
