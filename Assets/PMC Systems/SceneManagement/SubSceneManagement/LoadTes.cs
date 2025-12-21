using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

class LoadTes : MonoBehaviour
{
    public UnityEditor.SceneAsset SceneAsset;
}

class LoadTesBaker : Baker<LoadTes>
{
    public override void Bake(LoadTes authoring)
    {
        var reference = new EntitySceneReference(authoring.SceneAsset);
        var entity = GetEntity(TransformUsageFlags.None);
        
        AddComponent(entity, new test()
        {
            loadScene = false,
            scene = reference
        });
    }
}

public struct test : IComponentData
{
    public bool loadScene;
    public EntitySceneReference scene;
}