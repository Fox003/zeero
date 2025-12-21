using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class VFXEventSystem : SystemBase
{
    public NativeStream PendingStream;

    protected override void OnCreate()
    {
        
    }

    protected override void OnUpdate()
    {
        EntityQuery damagersQuery = GetEntityQuery(typeof(Damager));
        
        if (PendingStream.IsCreated)
        {
            PendingStream.Dispose(); 
        }
        
        PendingStream = new NativeStream(damagersQuery.CalculateChunkCount(), Allocator.TempJob);
        
        Dependency = new DamagersWriteToStreamJob
        {
            EntityType = GetEntityTypeHandle(),
            DamagerType = GetComponentTypeHandle<Damager>(false),
            StreamDamageEvents = PendingStream.AsWriter(),
        }.ScheduleParallel(damagersQuery, Dependency);
        
        /*
        Dependency = new SingleApplyStreamEventsToEntitiesJob
        {
            StreamDamageEvents = PendingStream.AsReader(),
            HealthFromEntity = GetComponentLookup<Health>(false),
        }.Schedule(Dependency);
        */
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (PendingStream.IsCreated)
        {
            PendingStream.Dispose();
        }
    }
}

public struct EventStreams : IComponentData
{
    public NativeStream DamageEventStream;
}


public struct StreamDamageEvent : IBufferElementData
{
    public Entity Target;
    public DamageEvent DamageEvent;
}

public struct DamageEvent : IBufferElementData
{
    public Entity Source;
    public float Value;
}

public struct Damager : IComponentData, IEnableableComponent
{
    public Entity Target;
    public float Damage;
}