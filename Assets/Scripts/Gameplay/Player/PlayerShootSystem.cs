using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerShootSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameFSM>();
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
        var gameState = SystemAPI.GetComponent<CurrentStateType>(gameFSM);

        if (gameState.Type != GameFSMStates.FIGHTING_STATE)
            return;
        
        var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (input, gunData, entity) in SystemAPI.Query<RefRO<PlayerInputs>, RefRW<GunData>>().WithEntityAccess())
        {
            if (input.ValueRO.PrimaryFire &&
                !gunData.ValueRO.CooldownData.isOnCooldown &&
                gunData.ValueRO.CurrentProjectile != Entity.Null)
            {
                var spawnedEntity = ecb.Instantiate(gunData.ValueRO.CurrentProjectile);
                var shootPosTransform = SystemAPI.GetComponent<LocalToWorld>(gunData.ValueRO.ShootPosition);
                ecb.SetComponent(spawnedEntity, new LocalTransform()
                {
                    Position = shootPosTransform.Position,
                    Rotation = shootPosTransform.Rotation,
                    Scale = 1
                });

                gunData.ValueRW.CooldownData.CurrentCooldownTime = 0;
            }
        }
    }
    

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}