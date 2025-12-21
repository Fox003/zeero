using Cysharp.Threading.Tasks;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;

partial class testsystem : SystemBase
{
    private SubSceneLoaderSystem _subSceneLoaderSystem;
    private SystemHandle _sceneSystem;
    
    protected override void OnCreate()
    {
        _subSceneLoaderSystem = World.GetExistingSystemManaged<SubSceneLoaderSystem>();

        _subSceneLoaderSystem.LoadingScene += OnSceneLoading;
        _subSceneLoaderSystem.SceneLoaded += OnSceneLoaded;
    }
    
    protected override void OnUpdate()
    {
        foreach (var test in SystemAPI.Query<RefRW<test>>())
        {
            if (test.ValueRO.loadScene)
            {
                test.ValueRW.loadScene = false;
                _ = _subSceneLoaderSystem.LoadSubscene(World.Unmanaged, test.ValueRO.scene, true, SceneLoadFlags.NewInstance);
            }
        }
    }

    public void OnSceneLoading()
    {
        Debug.Log("I know that a scene is loading...");
    }
    
    public void OnSceneLoaded()
    {
        Debug.Log("I know that a scene has finished loading...");
    }
    
    protected override void OnDestroy()
    {
        
    }

    public void Hello(WorldUnmanaged world, EntitySceneReference scene)
    {
        
    }
}
