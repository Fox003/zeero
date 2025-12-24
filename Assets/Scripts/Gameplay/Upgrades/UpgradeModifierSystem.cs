using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial struct UpgradeApplySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (mods, activeMods) in SystemAPI.Query<RefRW<PlayerStatsModifiers>, DynamicBuffer<ActiveModifier>>())
        {
            mods.ValueRW.Reset();

            foreach (var activeMod in activeMods)
            {
                mods.ValueRW.Add(activeMod.StatMods);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}