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
            // Round End
            if (conditions.ValueRO.IsTimeUp || conditions.ValueRO.IsPlayerDead)
            {
                var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFSM);
                var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);

                FSMUtilities.ChangeFSMState(gameFSM, gameAddBuffer, GameFSMStates.ROUND_END_STATE);
                FSMUtilities.ChangeFSMState(uiFsm, uiAddBuffer, UIFSMStates.HIDDEN_STATE);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}