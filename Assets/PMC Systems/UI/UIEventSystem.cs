using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
partial struct UIEventSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIScreens>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var screens = SystemAPI.GetSingleton<UIScreens>();
            
        // destroy all events that were created last frame
        var query = SystemAPI.QueryBuilder().WithAll<UIEvent>().Build();
        state.EntityManager.DestroyEntity(query);
            
        var ecb = screens.TestScreen1.Value.ECB;
        if (ecb.IsCreated)
        {
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        ecb = new EntityCommandBuffer(Allocator.TempJob);
        screens.TestScreen1.Value.ECB = ecb;
        screens.TestScreen2.Value.ECB = ecb;
        screens.GameStartScreen.Value.ECB = ecb;
    }

    public void OnDestroy(ref SystemState state)
    {
        var screens = SystemAPI.GetSingleton<UIScreens>();
        var ecb = screens.TestScreen2.Value.ECB;
        if (ecb.IsCreated)
        {
            ecb.Dispose();
        }
    }
}
