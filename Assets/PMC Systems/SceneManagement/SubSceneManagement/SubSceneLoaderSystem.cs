using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Entities.Serialization;

public partial class SubSceneLoaderSystem : SystemBase
{
    private static bool _isBusy = false;
    public event Action SceneLoaded;
    public event Action LoadingScene;

    protected override void OnStartRunning()
    {
        
    }

    protected override void OnCreate()
    {
        // noop
    }
    
    protected override void OnUpdate()
    {
        // noop
    }
    
    protected override void OnDestroy()
    {
        // noop
    }

    public async UniTask LoadSubscene(WorldUnmanaged world, EntitySceneReference entityScene, 
        bool autoLoad = true, 
        SceneLoadFlags loadFlag = SceneLoadFlags.LoadAdditive, 
        int priority = 0)
    {
        if (_isBusy)
        {
            Debug.Log("IS BUSY");
            return;
        }
        
        _isBusy = true;
        LoadingScene?.Invoke();
        
        var loadParameters = new SceneSystem.LoadParameters()
        {
            AutoLoad = false,
            Flags = loadFlag,
            Priority = 0
        };
        
        Entity handle = SceneSystem.LoadSceneAsync(world, entityScene, loadParameters);

        while (SceneSystem.GetSceneStreamingState(world, handle) != 
               SceneSystem.SceneStreamingState.LoadedSuccessfully)
        {
            Debug.Log(SceneSystem.GetSceneStreamingState(world, handle));
            await UniTask.Yield();
        }

        SceneLoaded?.Invoke();
        Debug.Log(SceneSystem.GetSceneStreamingState(world, handle));
        _isBusy = false;
    }
    
}
