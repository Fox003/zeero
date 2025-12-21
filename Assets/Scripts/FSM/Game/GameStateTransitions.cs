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
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
        
        var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);
        
        foreach (var (fsm, entity) in SystemAPI.Query<RefRO<GameFSM>>()
                     .WithChangeFilter<CurrentStateType>()
                     .WithEntityAccess())
        {
            // OnEnter GameStateCountdown
            if (SystemAPI.IsComponentEnabled<GameStateCountdown>(entity))
            {
                
            }
            // OnEnter GameStateFighting
            else if (SystemAPI.IsComponentEnabled<GameStateFighting>(entity))
            {
                
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
