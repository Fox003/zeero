using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public struct FSM : IComponentData {}
public struct FSMDefaultState : IComponentData, IEnableableComponent {}


public struct CurrentStateType : IComponentData
{
    public ComponentType Type;
}
public struct DisableStateRequest : IBufferElementData
{
    public Entity Entity;
    public ComponentType StateToDisable;
}

public struct EnableStateRequest : IBufferElementData
{
    public Entity Entity;
    public ComponentType StateToEnable;
    public bool IgnoreRequestFlag;
}

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct CommitStateTransitionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (fsm, entity) in SystemAPI.Query<RefRO<FSM>>()
                     .WithEntityAccess()
                     .WithAll<EnableStateRequest, DisableStateRequest>())
        {
            var addBuffer = SystemAPI.GetBuffer<EnableStateRequest>(entity);
            var removeBuffer = SystemAPI.GetBuffer<DisableStateRequest>(entity);
            
            foreach (var request in removeBuffer)
            {
                if (request.Entity == Entity.Null || request.StateToDisable == default)
                    continue;
                
                ecb.SetComponentEnabled(request.Entity, request.StateToDisable, false);
            }
        
            foreach (var request in addBuffer)
            {
                if (request.Entity == Entity.Null || request.StateToEnable == default || request.IgnoreRequestFlag)
                {
                    continue;
                }
                
                ecb.SetComponentEnabled(request.Entity, request.StateToEnable, true);
                ecb.SetComponent(request.Entity, new CurrentStateType(){ Type = request.StateToEnable });
            }
        
            addBuffer.Clear();
            removeBuffer.Clear();
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}