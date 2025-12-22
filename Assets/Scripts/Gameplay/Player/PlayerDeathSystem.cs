using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct PlayerDeathSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //var gameConditions = SystemAPI.GetSingletonRW<GameConditions>();

        foreach (var entityHealth in SystemAPI.Query<RefRO<HealthData>>()
            .WithChangeFilter<HealthData>())
        {
            if (entityHealth.ValueRO.CurrentHealth < 0f)
            {
                // Player is dead, do something
                SystemAPI.GetSingletonRW<GameConditions>().ValueRW.IsPlayerDead = true;
                Debug.Log("Player is dead");
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
