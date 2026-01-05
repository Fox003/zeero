using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

class LevelRegistryAuthoring : MonoBehaviour
{
    public List<UnityEditor.SceneAsset> LevelScenes;
}

class LevelRegistryAuthoringBaker : Baker<LevelRegistryAuthoring>
{
    public override void Bake(LevelRegistryAuthoring authoring)
    {
        var blobRef = BlobUtils.CreateBlobArrayRefFromList(
            authoring.LevelScenes,
            (sceneAsset, index) => new EntitySceneReference(sceneAsset)
        );

        AddComponent(GetEntity(TransformUsageFlags.None), new LevelRegistry
        {
            LevelsBlob = blobRef
        });
    }
}
