using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(GameplaySystemGroup))]
partial struct PlayerCooldownSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (weaponState, playerStats, playerModifiers) in SystemAPI.Query<RefRW<WeaponState>, RefRO<PlayerBaseStats>, RefRO<PlayerStatsModifiers>>())
        {
            var weaponStats = playerStats.ValueRO.WeaponStats;
            var modStats = playerModifiers.ValueRO.WeaponStatsModifiers;
            var modifiedCooldown = ModifierUtils.CalculateModifiedStat(weaponStats.CooldownTime, modStats.CooldownModifier);

            if (weaponState.ValueRO.CooldownData.CurrentCooldownTime < modifiedCooldown)
            {
                weaponState.ValueRW.CooldownData.CurrentCooldownTime += SystemAPI.Time.DeltaTime;
                weaponState.ValueRW.CooldownData.isOnCooldown = true;
            }
            else
            {
                weaponState.ValueRW.CooldownData.isOnCooldown = false;
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
