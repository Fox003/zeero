using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[BurstCompile(OptimizeFor = OptimizeFor.Performance)]
public partial struct ProjectileDeathStreamApplyJob : IJob
{
    public EntityCommandBuffer ECB;
    public NativeStream.Reader StreamProjectileDestroyEvents;

    public void Execute()
    {
        for (int i = 0; i < StreamProjectileDestroyEvents.ForEachCount; i++)
        {
            StreamProjectileDestroyEvents.BeginForEachIndex(i);
            while (StreamProjectileDestroyEvents.RemainingItemCount > 0)
            {
                StreamProjectileDestroyEvent streamDestroyEvent = StreamProjectileDestroyEvents.Read<StreamProjectileDestroyEvent>();
                ECB.DestroyEntity(streamDestroyEvent.EntityToDestroy);
            }
            StreamProjectileDestroyEvents.EndForEachIndex();
        }
    }
}
