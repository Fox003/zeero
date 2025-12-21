using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

[BurstCompile(OptimizeFor = OptimizeFor.Performance)]
public struct ProjectileDeathWriteToStreamJob : IJobChunk
{
    public ComponentTypeHandle<ProjectileDestroyRequest> DestroyRequestType;
    
    [ReadOnly] public EntityTypeHandle EntityTypeHandle;
    [NativeDisableParallelForRestriction] public NativeStream.Writer StreamDestroyRequestEvents;

    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
    {
        var chunkEntity = chunk.GetNativeArray(EntityTypeHandle);
        var enabledMask = chunk.GetEnabledMask(ref DestroyRequestType);

        StreamDestroyRequestEvents.BeginForEachIndex(unfilteredChunkIndex);
        
        var chunkCount = chunk.Count;
        for (var i = 0; i < chunkCount; i++)
        {
            if (!enabledMask[i])
                continue;
            
            StreamDestroyRequestEvents.Write(new StreamProjectileDestroyEvent
            {
                EntityToDestroy = chunkEntity[i],
            });
            
            enabledMask[i] = false;
        }
        
        StreamDestroyRequestEvents.EndForEachIndex();
        
    }
    
}

public struct StreamProjectileDestroyEvent : IComponentData 
{
    public Entity EntityToDestroy;
}
