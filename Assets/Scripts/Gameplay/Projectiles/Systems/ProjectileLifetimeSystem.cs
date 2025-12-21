using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial struct ProjectileLifetimeSystem : ISystem
{
    public ComponentLookup<ProjectileDestroyRequest> _destroyRequestLookup;
    private EntityQuery _projectiles;

    public void OnCreate(ref SystemState state)
    {
        _projectiles = state.GetEntityQuery(ComponentType.ReadWrite<ProjectileLifeData>());
        _destroyRequestLookup = state.GetComponentLookup<ProjectileDestroyRequest>(false);
    }

    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        
        _destroyRequestLookup.Update(ref state);

        var job = new ProjectileLifeJob
        {
            DestroyRequestLookup = _destroyRequestLookup,
            DeltaTime = dt,
        };

        state.Dependency = job.Schedule(_projectiles, state.Dependency);
    }
    
    public partial struct ProjectileLifeJob : IJobEntity
    {
        public ComponentLookup<ProjectileDestroyRequest> DestroyRequestLookup;
        public EntityCommandBuffer.ParallelWriter ECB;
        public float DeltaTime;

        private void Execute(Entity entity, ref ProjectileLifeData life)
        {
            life.CurrentLifetime += DeltaTime;
            
            if (life.CurrentLifetime >= life.Lifetime)
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
