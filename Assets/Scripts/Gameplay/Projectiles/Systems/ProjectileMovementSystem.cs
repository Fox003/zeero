using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct ProjectileMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (movementData, transform, entity) in SystemAPI
                     .Query<RefRO<ProjectileMovementData>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            float3 forward = math.mul(transform.ValueRO.Rotation, new float3(-1, 0, 0));
            transform.ValueRW = transform.ValueRW.Translate(forward * movementData.ValueRO.MovementSpeed * SystemAPI.Time.DeltaTime);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
