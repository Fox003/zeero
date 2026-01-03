using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(GameplaySystemGroup))]
partial struct PlayerLookSystem : ISystem
{
    private float lastAngle;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, inputs) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerInputs>>())
        {
            var lookDirection = new float2();
            
            if (inputs.ValueRO.isGamepad)
            {
                lookDirection = transform.ValueRO.Position.xy - (transform.ValueRO.Position.xy + inputs.ValueRO.lookInput);
            }
            else
            {
                lookDirection = transform.ValueRO.Position.xy - inputs.ValueRO.mouseLookInput;
            }
            
            

            if (math.lengthsq(lookDirection) > 0.2f)
            {
                var angle = math.atan2(lookDirection.y, lookDirection.x);
                transform.ValueRW.Rotation = quaternion.Euler(0, 0, angle);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
