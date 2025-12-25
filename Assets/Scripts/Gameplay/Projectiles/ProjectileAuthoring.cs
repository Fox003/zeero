using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ProjectileAuthoring : MonoBehaviour
{
    public ParticleSystem DeathVFX;
    public ParticleSystem SpawnVFX;
}

class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(entity, new ProjectileData()
        {
            Lifetime = 99
        });

        AddComponent(entity, new ProjectileState()
        {
            CurrentLifetime = 0,
        });
        
        AddComponent<ProjectileDestroyRequest>(entity);
        
        SetComponentEnabled<ProjectileDestroyRequest>(entity, false);
    }
}
