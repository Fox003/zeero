using Unity.Burst;
using Unity.Entities;

partial struct ConditionsWriterSystem : ISystem
{
    

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameConditions>();
        state.RequireForUpdate<GameTimerData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameTimer = SystemAPI.GetSingleton<GameTimerData>();
        

        if (gameTimer.CurrentTime < 0)
        {
            var gameConditions = SystemAPI.GetSingletonRW<GameConditions>();
            var timerRW = SystemAPI.GetSingletonRW<GameTimerData>();
            
            gameConditions.ValueRW.IsTimeUp = true;
            timerRW.ValueRW.Reset();
        }

        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
