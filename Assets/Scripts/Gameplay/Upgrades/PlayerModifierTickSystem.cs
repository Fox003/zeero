using System;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateBefore(typeof(PlayerStatModifierSystem))]
partial struct PlayerModifierTickSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var buffer in SystemAPI.Query<DynamicBuffer<ActiveModifier>>())
        {
            // Backwards for loop because we remove items during the iteration
            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                var mod = buffer[i];

                if (mod.Duration > 0)
                {
                    mod.Duration -= deltaTime;

                    if (mod.Duration <= 0)
                    {
                        buffer.RemoveAt(i);
                        continue;
                    }

                    buffer.ElementAt(i) = mod;
                }
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
