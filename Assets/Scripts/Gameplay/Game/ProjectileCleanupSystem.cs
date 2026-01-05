using Unity.Burst;
using Unity.Entities;

partial struct ProjectileCleanupSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
        var gameState = SystemAPI.GetComponent<CurrentStateType>(gameFSM);

        if (gameState.Type != GameFSMStates.ROUND_END_STATE)
            return;

        foreach (var (projectile, entity) in SystemAPI.Query<RefRO<ProjectileData>>().WithEntityAccess())
        {
            SystemAPI.SetComponentEnabled<ProjectileDestroyRequest>(entity, true);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
