using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct PlayerStatModifierSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerMods, activeMods) in SystemAPI.Query<RefRW<PlayerStatsModifiers>, DynamicBuffer<ActiveModifier>>())
        {
            playerMods.ValueRW.Reset();

            foreach (var activeMod in activeMods)
            {
                playerMods.ValueRW.Add(activeMod.StatMods);
                playerMods.ValueRW.Scale(activeMod.Count);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}