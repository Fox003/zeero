using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(CommitStateTransitionSystem))]
partial struct GameStateTransitionValidation : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameFSM>();
        state.RequireForUpdate<GameStateTransitionMap>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameFsm = SystemAPI.GetSingletonEntity<GameFSM>();
        var gameTransitionMap = SystemAPI.GetSingleton<GameStateTransitionMap>();
        
        var removeBuffer = SystemAPI.GetBuffer<DisableStateRequest>(gameFsm);
        var addBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFsm);

        ComponentType currentState = SystemAPI.GetComponent<CurrentStateType>(gameFsm).Type;
        
        // Checking if the transition that is requested is valid
        FSMUtilities.ValidateTransition(ref removeBuffer, ref addBuffer, ref gameTransitionMap.Transitions.Value.Pairs, currentState, gameFsm);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
