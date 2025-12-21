using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[BurstCompile(OptimizeFor = OptimizeFor.Performance)]
public struct SingleApplyStreamEventsToEntitiesJob : IJob
{
    /*public NativeStream.Reader StreamDamageEvents;
    public ComponentLookup<Health> HealthFromEntity;*/

    public void Execute()
    {
        /*for (int i = 0; i < StreamDamageEvents.ForEachCount; i++)
        {
            StreamDamageEvents.BeginForEachIndex(i);
            while (StreamDamageEvents.RemainingItemCount > 0)
            {
                StreamDamageEvent streamDamageEvent = StreamDamageEvents.Read<StreamDamageEvent>();
                if (HealthFromEntity.HasComponent(streamDamageEvent.Target))
                {
                    Health health = HealthFromEntity[streamDamageEvent.Target];
                    health.Value -= streamDamageEvent.DamageEvent.Value;
                    HealthFromEntity[streamDamageEvent.Target] = health;
                }
                
                
                Debug.Log($"TARGET: {streamDamageEvent.Target}, DAMAGE: {streamDamageEvent.DamageEvent.Value}");
            }
            StreamDamageEvents.EndForEachIndex();
        }*/
    }
}
