using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct ProjectileDeathSystem : ISystem
{
    private NativeStream _pendingStream;
    private EntityQuery _toBeDestroyedQuery;
    private EntityTypeHandle _entityType;
    private ComponentTypeHandle<ProjectileDestroyRequest> _componentType;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        _entityType = state.GetEntityTypeHandle();
        _componentType = state.GetComponentTypeHandle<ProjectileDestroyRequest>();
        
        _toBeDestroyedQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ProjectileDestroyRequest>()
            .Build(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
        
        _entityType.Update(ref state);
        _componentType.Update(ref state);

        if (_pendingStream.IsCreated)
        {
            _pendingStream.Dispose();
        }
        
        _pendingStream = new NativeStream(_toBeDestroyedQuery.CalculateChunkCountWithoutFiltering(), Allocator.TempJob);

        state.Dependency = new ProjectileDeathWriteToStreamJob()
        {
            EntityTypeHandle = _entityType,
            DestroyRequestType = _componentType,
            StreamDestroyRequestEvents = _pendingStream.AsWriter(),
        }.ScheduleParallel(_toBeDestroyedQuery, state.Dependency);

        state.Dependency = new ProjectileDeathStreamApplyJob()
        {
            ECB = ecb,
            StreamProjectileDestroyEvents = _pendingStream.AsReader()
        }.Schedule(state.Dependency);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        _pendingStream.Dispose();
    }
}

