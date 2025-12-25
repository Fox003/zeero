using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    [Header("Movement")]
    public float MaxMoveSpeed;
    public float Acceleration;
    public float Drag;

    [Header("Vitals")]
    public float MaxHealth;
    public float MaxShield;

    [Header("Weapon")]
    public float Cooldown;
    public float Damage;
    public Transform ShootTransform;

    [Header("Projectile")]
    public GameObject Projectile;
    public float ProjectileSpeed;
    public float ProjectileSize;
    public float ProjectileDuration;

}


class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var proj = GetEntity(authoring.Projectile, TransformUsageFlags.Dynamic);
        var shootTransform = GetEntity(authoring.ShootTransform, TransformUsageFlags.Dynamic);
        
        // TAGS

        AddComponent<PlayerNeedsInputAssociation>(entity);
        AddComponent<LookData>(entity);
        
        AddComponentObject(entity, new ControllerReference());

        // COMPONENTS INIT
        
        AddComponent(entity, new PlayerInputs()
        {
            moveInput = new float2(0, 0),
        });

        AddComponent(entity, new PlayerBaseStats()
        {
            MovementStats = new MovementStats() 
            {
                MaxMoveSpeed = authoring.MaxMoveSpeed,
                Acceleration = authoring.Acceleration,
                Drag = authoring.Drag,
            },

            HealthStats = new HealthStats()
            {
                MaxHealth = authoring.MaxHealth,
                MaxShield = authoring.MaxShield,
            },

            WeaponStats = new WeaponStats()
            {
                ProjectileSize = authoring.ProjectileSize,
                ProjectileSpeed = authoring.ProjectileSpeed,
                CooldownTime = authoring.Cooldown,
                Damage = authoring.Damage,
                ProjectileLifetime = authoring.ProjectileDuration
            }
        });
        
        AddComponent(entity, new MovementState()
        { 
            CurrentMoveDirection = float3.zero,
            DesiredMoveDirection = float3.zero
        });

        AddComponent(entity, new WeaponState()
        {
            CurrentProjectile = proj,
            ShootPosition = shootTransform
        });
        
        AddComponent(entity, new HealthState()
        {
            CurrentHealth = authoring.MaxHealth,
            CurrentShield = authoring.MaxShield
        });
    }
}
