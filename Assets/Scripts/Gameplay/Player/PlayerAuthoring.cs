using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    [Header("Movement")]
    public float MaxMoveSpeed;
    public float Acceleration;
    public float Drag;

    public float MaxHealth;
    
    [Header("Shooting")]
    public float ShootCooldown;
    public GameObject defaultProjectile;
    public Transform ShootTransform;
}


class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var proj = GetEntity(authoring.defaultProjectile, TransformUsageFlags.Dynamic);
        var shootTransform = GetEntity(authoring.ShootTransform, TransformUsageFlags.Dynamic);
        
        AddComponent<PlayerNeedsInputAssociation>(entity);
        AddComponent<LookData>(entity);
        AddComponent<EntitySpawnRequest>(entity);
        SetComponentEnabled<EntitySpawnRequest>(entity,false);
        
        AddComponentObject(entity, new ControllerReference());
        
        AddComponent(entity, new PlayerInputs()
        {
            moveInput = new float2(0, 0),
        });
        
        AddComponent(entity, new MovementData()
        { 
            MaxMoveSpeed = authoring.MaxMoveSpeed,
            Acceleration =  authoring.Acceleration,
            CurrentMoveDirection= float3.zero,
            Drag = authoring.Drag,
            DesiredMoveDirection = float3.zero
        });
        
        AddComponent(entity, new GunData()
        {
            ShootPosition = shootTransform,
            CurrentProjectile = proj,
            CooldownData = new CooldownData()
            {
                CooldownTime = authoring.ShootCooldown,
                CurrentCooldownTime = 0
            }
        });
        
        AddComponent(entity, new HealthData()
        {
            MaxHealth = authoring.MaxHealth,
            CurrentHealth = authoring.MaxHealth
        });
        
        
    }
}
