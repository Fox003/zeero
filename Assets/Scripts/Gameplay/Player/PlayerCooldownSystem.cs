using Unity.Burst;
using Unity.Entities;

partial struct PlayerCooldownSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var gunData in SystemAPI.Query<RefRW<GunData>>())
        {
            if (gunData.ValueRO.CooldownData.CurrentCooldownTime < gunData.ValueRO.CooldownData.CooldownTime)
            {
                gunData.ValueRW.CooldownData.CurrentCooldownTime += SystemAPI.Time.DeltaTime;
                gunData.ValueRW.CooldownData.isOnCooldown = true;
            }
            else
            {
                gunData.ValueRW.CooldownData.isOnCooldown = false;
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
