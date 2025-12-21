using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile(OptimizeFor = OptimizeFor.Performance)]
public struct DamagersWriteToStreamJob : IJobChunk
{
    public ComponentTypeHandle<Damager> DamagerType;
    
    [ReadOnly] public EntityTypeHandle EntityType;
    [NativeDisableParallelForRestriction] public NativeStream.Writer StreamDamageEvents;

    public void Execute(in ArchetypeChunk chunk, Int32 unfilteredChunkIndex, Boolean useEnabledMask,
        in v128 chunkEnabledMask)
    {
        var chunkEntity = chunk.GetNativeArray(EntityType);
        var chunkDamager = chunk.GetNativeArray(ref DamagerType);
        var enabledMask = chunk.GetEnabledMask(ref DamagerType);

        StreamDamageEvents.BeginForEachIndex(unfilteredChunkIndex);

        var chunkCount = chunk.Count;
        for (var i = 0; i < chunkCount; i++)
        {
            StreamDamageEvents.Write(new StreamDamageEvent
            {
                Target = chunkDamager[i].Target,
                DamageEvent = new DamageEvent
                {
                    Source = chunkEntity[i],
                    Value = chunkDamager[i].Damage,
                },
            });

            enabledMask.EnableBit.SetBit(false);
        }

        StreamDamageEvents.EndForEachIndex();
    }
}
