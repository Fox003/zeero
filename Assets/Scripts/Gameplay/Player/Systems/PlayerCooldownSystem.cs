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
        foreach (var (weaponState, playerStats) in SystemAPI.Query<RefRW<WeaponState>, RefRO<PlayerBaseStats>>())
        {
            var weaponStats = playerStats.ValueRO.WeaponStats;

            if (weaponState.ValueRO.CooldownData.CurrentCooldownTime < weaponStats.CooldownTime)
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
