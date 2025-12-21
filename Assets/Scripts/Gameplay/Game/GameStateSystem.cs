using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial struct GameStateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIFSM>();
        state.RequireForUpdate<GameFSM>();
        state.RequireForUpdate<GameConditions>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
        var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();

        foreach (var conditions in SystemAPI.Query<RefRO<GameConditions>>().WithChangeFilter<GameConditions>())
        {
            if (conditions.ValueRO.start)
            {
                var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFSM);
                var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);
            
                gameAddBuffer.Add(new EnableStateRequest()
                {
                    Entity = gameFSM,
                    StateToEnable = GameFSMStates.COUNTDOWN_STATE,
                });
            
                uiAddBuffer.Add(new EnableStateRequest()
                {
                    Entity = uiFsm,
                    StateToEnable = UIFSMStates.GAME_STARTING_STATE
                });
            }
            
            // Round End
            if (conditions.ValueRO.IsTimeUp || conditions.ValueRO.IsPlayerDead)
            {
                var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFSM);
                var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);
            
                gameAddBuffer.Add(new EnableStateRequest()
                {
                    Entity = gameFSM,
                    StateToEnable = GameFSMStates.ROUND_END_STATE,
                });
            
                uiAddBuffer.Add(new EnableStateRequest()
                {
                    Entity = uiFsm,
                    StateToEnable = UIFSMStates.HIDDEN_STATE
                });
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public enum GameState
{
    Starting,
    Fighting,
    Upgrading,
    GameOver
}