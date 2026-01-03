using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(GameplaySystemGroup))]

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
        
        foreach (var (input, playerStats, weaponState, modifiers, entity) in SystemAPI
            .Query<RefRO<PlayerInputs>, 
            RefRO<PlayerBaseStats>, 
            RefRW<WeaponState>, 
            RefRO<PlayerStatsModifiers>>().WithEntityAccess())
        {
            var weaponStats = playerStats.ValueRO.WeaponStats;
            var weaponMods = modifiers.ValueRO.WeaponStatsModifiers;

            if (input.ValueRO.PrimaryFire &&
                !weaponState.ValueRO.CooldownData.isOnCooldown &&
                weaponState.ValueRO.CurrentProjectile != Entity.Null)
            {
                var spawnedEntity = ecb.Instantiate(weaponState.ValueRO.CurrentProjectile);
                var shootPosTransform = SystemAPI.GetComponent<LocalToWorld>(weaponState.ValueRO.ShootPosition);

                ecb.SetComponent(spawnedEntity, new LocalTransform()
                {
                    Position = shootPosTransform.Position,
                    Rotation = shootPosTransform.Rotation,
                    Scale = ModifierUtils.CalculateModifiedStat(weaponStats.ProjectileSize, weaponMods.ProjectileSizeModifier)
                });

                ecb.SetComponent(spawnedEntity, new ProjectileData()
                {
                    MovementSpeed = ModifierUtils.CalculateModifiedStat(weaponStats.ProjectileSpeed, weaponMods.ProjectileSpeedModifier),
                    Lifetime = ModifierUtils.CalculateModifiedStat(weaponStats.ProjectileLifetime, weaponMods.ProjectileLifetimeModifier),
                    Damage = ModifierUtils.CalculateModifiedStat(weaponStats.Damage, weaponMods.DamageModifier)
                });


                weaponState.ValueRW.CooldownData.CurrentCooldownTime = 0;
            }
        }
    }
    

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}