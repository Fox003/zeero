using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
partial struct ProjectileCollisionSystem : ISystem
{
    private ComponentDataHandles _componentDataHandles;

    internal struct ComponentDataHandles
    {
        public ComponentLookup<ProjectileDamageData> ProjectileDamageLookup;
        public ComponentLookup<HealthData> HealthLookup;
        public ComponentLookup<ProjectileDestroyRequest> DestroyRequestLookup;

        public ComponentDataHandles(ref SystemState systemState)
        {
            ProjectileDamageLookup = systemState.GetComponentLookup<ProjectileDamageData>(true);
            HealthLookup = systemState.GetComponentLookup<HealthData>(false);
            DestroyRequestLookup = systemState.GetComponentLookup<ProjectileDestroyRequest>(false);
        }

        public void Update(ref SystemState systemState)
        {
            ProjectileDamageLookup.Update(ref systemState);
            HealthLookup.Update(ref systemState);
            DestroyRequestLookup.Update(ref systemState);
        }
    }
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<SimulationSingleton>();
        _componentDataHandles = new ComponentDataHandles(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _componentDataHandles.Update(ref state);
        
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecbParallel = ecbSingleton
            .CreateCommandBuffer(state.WorldUnmanaged)
            .AsParallelWriter();
        
        state.Dependency = new ProjectileCollisionJob()
        {
            ProjectileDamageLookup = _componentDataHandles.ProjectileDamageLookup,
            HealthLookup = _componentDataHandles.HealthLookup,
            DestroyRequestLookup = _componentDataHandles.DestroyRequestLookup,
            ECB = ecbParallel
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public partial struct ProjectileCollisionJob : ICollisionEventsJob
{
    [ReadOnly] public ComponentLookup<ProjectileDamageData> ProjectileDamageLookup;
    public ComponentLookup<HealthData> HealthLookup;
    public ComponentLookup<ProjectileDestroyRequest> DestroyRequestLookup;
    public EntityCommandBuffer.ParallelWriter ECB;
    
    public void Execute(CollisionEvent collisionEvent)
    {
        var entityA = collisionEvent.EntityA;
        var entityB = collisionEvent.EntityB;
        
        var bodyAHasHealth = HealthLookup.HasComponent(entityA);
        var bodyBHasHealth = HealthLookup.HasComponent(entityB);
        
        var bodyAisProjectile = ProjectileDamageLookup.HasComponent(entityA);
        var bodyBisProjectile = ProjectileDamageLookup.HasComponent(entityB);

        if (bodyBisProjectile && bodyAHasHealth)
        {
            DamageEntity(entityA, entityB);
            DestroyRequestLookup.SetComponentEnabled(entityB, true);
        }
        else if (bodyBHasHealth && bodyAisProjectile)
        {
            DamageEntity(entityB, entityA);
            DestroyRequestLookup.SetComponentEnabled(entityA, true);
        }
        else if (bodyBisProjectile && bodyAisProjectile)
        {
            
        }
    }

    private void DamageEntity(Entity damagedEntity, Entity damagerEntity)
    {
        var damageData = ProjectileDamageLookup[damagerEntity];
        var healthData = HealthLookup[damagedEntity];
        
        healthData.CurrentHealth -= damageData.Damage;
        HealthLookup[damagedEntity] = healthData;
    }
}
