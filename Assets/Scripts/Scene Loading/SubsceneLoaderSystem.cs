using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct SubsceneLoaderSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var unloadReqList = new NativeList<SceneUnloadRequest>(Allocator.Temp);
        var loadReqList = new NativeList<SceneLoadRequest>(Allocator.Temp);

        // --- UNLOAD HANDLER ---
        foreach (var (unload, reqEntity) in SystemAPI.Query<SceneUnloadRequest>().WithEntityAccess())
        {
            unloadReqList.Add(unload);
            ecb.DestroyEntity(reqEntity);
        }

        // --- LOAD HANDLER ---
        foreach (var (load, reqEntity) in SystemAPI.Query<SceneLoadRequest>().WithEntityAccess())
        {
            loadReqList.Add(load);
            ecb.DestroyEntity(reqEntity);
        }

        ProcessScenesToLoad(loadReqList, state.WorldUnmanaged, ref state);
        ProcessScenesToUnload(unloadReqList, state.WorldUnmanaged, ref state);

        loadReqList.Dispose();
        unloadReqList.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    private void ProcessScenesToLoad(NativeList<SceneLoadRequest> scenesToLoad, WorldUnmanaged world, ref SystemState state)
    {
        foreach (var req in scenesToLoad)
        {
            var newSceneEntity = SceneSystem.LoadSceneAsync(world, req.SceneReference);

            if (SystemAPI.TryGetSingletonRW<CurrentLevelState>(out var currentLevel))
            {
                currentLevel.ValueRW.Level = newSceneEntity;
            }
        }
    }

    private void ProcessScenesToUnload(NativeList<SceneUnloadRequest> scenesToUnload, WorldUnmanaged world, ref SystemState state)
    {
        foreach (var req in scenesToUnload)
        {
            if (SystemAPI.Exists(req.SceneEntity))
            {
                SceneSystem.UnloadScene(world, req.SceneEntity);
            }
        }
    }
}

public struct SceneUnloadRequest : IComponentData
{
    public Entity SceneEntity;
}

public struct SceneLoadRequest : IComponentData
{
    public EntitySceneReference SceneReference;
}