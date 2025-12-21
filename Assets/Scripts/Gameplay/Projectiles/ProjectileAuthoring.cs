using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ProjectileAuthoring : MonoBehaviour
{
    public float Lifetime;
    public float MovementSpeed;
    public ParticleSystem DeathVFX;
    public ParticleSystem SpawnVFX;

    public float Damage;
}

class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(entity, new ProjectileMovementData()
        {
            MovementBehavior = MovementBehavior.Straight,
            MovementSpeed = authoring.MovementSpeed,
            TargetPosition = new float3(2, 9, 0)
        });
        
        AddComponent(entity, new ProjectileLifeData()
        {
            Lifetime = authoring.Lifetime,
            CurrentLifetime = 0
        });
        
        AddComponent(entity, new ProjectileDamageData()
        {
            Damage = authoring.Damage,
        });
        
        AddComponent<ProjectileDestroyRequest>(entity);
        
        SetComponentEnabled<ProjectileDestroyRequest>(entity, false);
    }
}
