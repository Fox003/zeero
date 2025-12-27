using Unity.Entities;
using UnityEngine;

public class ModifierUtils
{
    public static float CalculateModifiedStat(float baseStat, Modifier modifier)
    {
        return baseStat * (1.0f + (modifier.MultiplyMod/100f)) + modifier.AdditiveMod;
    }

    public static void AddModifier(DynamicBuffer<ActiveModifier> activeModsBuffer, UpgradeDefinition upgradeData)
    {
        bool alreadyExists = false;

        for (int i = 0; i < activeModsBuffer.Length; i++)
        {
            if (activeModsBuffer[i].ID == upgradeData.ID)
            {
                var existingMod = activeModsBuffer[i];
                existingMod.Count += 1;

                activeModsBuffer[i] = existingMod;

                alreadyExists = true;
                break;
            }
        }

        if (!alreadyExists)
        {
            activeModsBuffer.Add(new ActiveModifier()
            {
                Count = 1,
                Duration = upgradeData.Duration,
                ID = upgradeData.ID,
                StatMods = upgradeData.StatMods
            });
        }
    }
}
