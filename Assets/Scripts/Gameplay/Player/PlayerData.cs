using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct MovementData : IComponentData
{
    public float MaxMoveSpeed;
    public float Acceleration;
    public float Drag;
    public float3 DesiredMoveDirection;
    public float3 CurrentMoveDirection;
}

public struct LookData : IComponentData
{
    public float lastAngle;
}

public struct HealthData : IComponentData
{
    public float MaxHealth;
    public float CurrentHealth;
    public float MaxShield;
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

public struct GunData : IComponentData
{
    public Entity CurrentProjectile;
    public Entity ShootPosition;
    public CooldownData CooldownData;
}

public struct CooldownData : IComponentData
{
    public bool isOnCooldown;
    public float CooldownTime;
    public float CurrentCooldownTime;
}

public struct EntitySpawnRequest : IComponentData, IEnableableComponent
{
    
}

public struct PlayerNeedsInputAssociation : IComponentData, IEnableableComponent { }