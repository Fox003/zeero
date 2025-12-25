using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

partial struct ProjectileLifetimeSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (projState, projData, entity) in SystemAPI.Query<RefRW<ProjectileState>, RefRO<ProjectileData>>().WithEntityAccess())
        {

            projState.ValueRW.CurrentLifetime += SystemAPI.Time.DeltaTime;

            if (projState.ValueRO.CurrentLifetime >= projData.ValueRO.Lifetime)
            {
                SystemAPI.SetComponentEnabled<ProjectileDestroyRequest>(entity, true);
            }
        }
    }
    
    public partial struct ProjectileLifeJob : IJobEntity
    {
        public ComponentLookup<ProjectileDestroyRequest> DestroyRequestLookup;
        public EntityCommandBuffer.ParallelWriter ECB;
        public float DeltaTime;

        private void Execute(Entity entity, ref ProjectileState life, ref ProjectileData projData)
        {
            life.CurrentLifetime += DeltaTime;
            
            if (life.CurrentLifetime >= projData.Lifetime)
            {
                DestroyRequestLookup.SetComponentEnabled(entity, true);
            }
        }
    } 

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
