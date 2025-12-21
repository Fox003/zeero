using Unity.Burst;
using Unity.Entities;

partial struct GameTimerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameTimerData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameTimer = SystemAPI.GetSingletonRW<GameTimerData>();

        if (!gameTimer.ValueRO.IsPaused && gameTimer.ValueRO.CurrentTime > 0)
        {
            gameTimer.ValueRW.CurrentTime -= SystemAPI.Time.DeltaTime;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
