using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(CommitStateTransitionSystem))]
partial struct UIStateTransitionValidation : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIFSM>();
        state.RequireForUpdate<UIStateTransitionMap>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
        var uiTransitionMap = SystemAPI.GetSingleton<UIStateTransitionMap>();
        
        var removeBuffer = SystemAPI.GetBuffer<DisableStateRequest>(uiFsm);
        var addBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);

        ComponentType currentState = SystemAPI.GetComponent<CurrentStateType>(uiFsm).Type;
        
        // Checking if the transition that is requested is valid
        FSMUtilities.ValidateTransition(ref removeBuffer, ref addBuffer, ref uiTransitionMap.Transitions.Value, currentState, uiFsm);
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
