using Unity.Entities;

public struct HealthModifier : IComponentData
{
    public Modifier MaxHealthModifier;
    public Modifier MaxShieldModifier;
}

public struct MovementModifier : IComponentData
{
    public Modifier MoveSpeedModifier;
    public Modifier AccelerationModifier;
}
public struct WeaponModifier : IComponentData
{
    public Modifier CooldownModifier;
}

public struct Modifier
{
    public float AdditiveMod;
    public float MultiplyMod;
}
