using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct GameFightingViewUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }
    
    public void OnUpdate(ref SystemState state)
    {


        var screens = SystemAPI.GetSingletonRW<UIScreens>();
        var viewModel = SystemAPI.ManagedAPI.GetSingleton<GameFightingViewModel>();
        var gameTimer = SystemAPI.GetSingleton<GameTimerData>();

        viewModel.RawTimeInSeconds = math.trunc(gameTimer.CurrentTime);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
