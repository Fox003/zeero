using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct UISystem : ISystem
{
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIFSM>();
        state.RequireForUpdate<GameFSM>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var gameFsm = SystemAPI.GetSingletonEntity<GameFSM>();
        var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();

        foreach (var stateChangeEvent in SystemAPI.Query<RefRO<UIStateChangeEvent>>())
        {
            var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFsm);
            var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);
            
            gameAddBuffer.Add(new EnableStateRequest()
            {
                Entity = gameFsm,
                IgnoreRequestFlag = false,
                StateToEnable = stateChangeEvent.ValueRO.NewGameState,
            });
            
            uiAddBuffer.Add(new EnableStateRequest()
            {
                Entity = uiFsm,
                IgnoreRequestFlag = false,
                StateToEnable = stateChangeEvent.ValueRO.NewUIState
            });
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public struct UIInterfaceEvent : IComponentData
{
    public InterfaceState InterfaceState;
}

public struct UIStateChangeEvent : IComponentData
{
    public ComponentType NewGameState;
    public ComponentType NewUIState;
}

public enum InterfaceState
{
    TestScreen1,
    TestScreen2,
    GameStartScreen,
    Hidden,
}
