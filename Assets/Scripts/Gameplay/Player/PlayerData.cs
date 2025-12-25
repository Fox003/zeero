using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct PlayerBaseStats : IComponentData
{
    public MovementStats MovementStats;
    public HealthStats HealthStats;
    public WeaponStats WeaponStats;
}


[System.Serializable]
public struct MovementStats
{
    public float MaxMoveSpeed;
    public float Acceleration;
    public float Drag;
}

[System.Serializable]
public struct HealthStats
{
    public float MaxHealth;
    public float MaxShield;
}

[System.Serializable]
public struct WeaponStats
{
    public float CooldownTime;
    public float ProjectileSpeed;
    public float ProjectileSize;
    public float ProjectileLifetime;
    public float Damage;
}

public struct WeaponState : IComponentData
{
    public CooldownData CooldownData;
    public Entity CurrentProjectile;
    public Entity ShootPosition;
}


public struct MovementState : IComponentData
{
    public float3 DesiredMoveDirection;
    public float3 CurrentMoveDirection;
}

public struct HealthState : IComponentData
{
    public float CurrentHealth;
    public float CurrentShield;
}



// -----------------------------------------------------

public struct LookData : IComponentData
{
    public float lastAngle;
}

public struct PlayerInputs : IComponentData, IEnableableComponent
{
    public bool isGamepad;
    public float2 moveInput;
    public float2 lookInput;
    public float2 mouseLookInput;
    public bool PrimaryFire;
}

public class ControllerReference : IComponentData
{
    public PlayerInputReference PlayerInputRef;
}

public struct CooldownData : IComponentData
{
    public bool isOnCooldown;
    public float CurrentCooldownTime;
}


public struct PlayerNeedsInputAssociation : IComponentData, IEnableableComponent { }