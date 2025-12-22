using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(CommitStateTransitionSystem))]
[UpdateBefore(typeof(UIStateTransitions))]
partial struct GameStateTransitions : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIFSM>();
        state.RequireForUpdate<GameFSM>();
        state.RequireForUpdate<GameTimerData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (fsm, entity) in SystemAPI.Query<RefRO<GameFSM>>()
                     .WithChangeFilter<CurrentStateType>()
                     .WithEntityAccess())
        {
            if (SystemAPI.IsComponentEnabled<GameStateInit>(entity))
            {
                var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
                var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();

                var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);
                var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFSM);

                GameFSMUtilities.OnInitStateEnter(uiFsm, gameFSM, uiAddBuffer, gameAddBuffer);
            }
            // OnEnter GameStateFighting
            else if (SystemAPI.IsComponentEnabled<GameStateFighting>(entity))
            {
                // Start the timer
                var timerData = SystemAPI.GetSingletonRW<GameTimerData>();

                GameFSMUtilities.OnFightingStateEnter(ref timerData.ValueRW);
            }
        }
    }

    

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
