using UnityEngine;

public class ModifierUtils
{
    public static float CalculateModifiedStat(float baseStat, Modifier modifier)
    {
        return baseStat * (1.0f + modifier.MultiplyMod) + modifier.AdditiveMod;
    }
}
