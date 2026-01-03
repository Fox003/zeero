using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct PlayerStatsModifiers : IComponentData
{
    public MovementModifiers MovementStatsModifiers;
    public HealthModifiers HealthStatsModifiers;
    public WeaponModifiers WeaponStatsModifiers;

    public void Reset()
    {
        this.MovementStatsModifiers.Reset();
        this.HealthStatsModifiers.Reset();
        this.WeaponStatsModifiers.Reset();
    }

    public void Add(PlayerStatsModifiers other)
    {
        this.MovementStatsModifiers.Add(other.MovementStatsModifiers);
        this.HealthStatsModifiers.Add(other.HealthStatsModifiers);
        this.WeaponStatsModifiers.Add(other.WeaponStatsModifiers);
    }

    public void Scale(int scale)
    {
        this.MovementStatsModifiers.Scale(scale);
        this.HealthStatsModifiers.Scale(scale);
        this.WeaponStatsModifiers.Scale(scale);
    }
}

[System.Serializable]
public struct MovementModifiers
{
    public Modifier MaxMoveSpeedModifier;
    public Modifier AccelerationModifier;
    public Modifier DragModifier;

    public void Add(MovementModifiers other)
    {
        this.MaxMoveSpeedModifier.Add(other.MaxMoveSpeedModifier);
        this.AccelerationModifier.Add(other.AccelerationModifier);
        this.DragModifier.Add(other.DragModifier);
    }

    public void Scale(int scale)
    {
        this.MaxMoveSpeedModifier.Scale(scale);
        this.AccelerationModifier.Scale(scale);
        this.DragModifier.Scale(scale);
    }

    public void Reset()
    {
        this.MaxMoveSpeedModifier.Reset();
        this.AccelerationModifier.Reset();
        this.DragModifier.Reset();
    }
}

[System.Serializable]
public struct HealthModifiers
{
    public Modifier MaxHealthModifier;
    public Modifier MaxShieldModifier;

    public void Add(HealthModifiers other)
    {
        this.MaxHealthModifier.Add(other.MaxHealthModifier);
        this.MaxShieldModifier.Add(other.MaxShieldModifier);
    }

    public void Scale(int scale)
    {
        this.MaxHealthModifier.Scale(scale);
        this.MaxShieldModifier.Scale(scale);
    }

    public void Reset()
    {
        this.MaxHealthModifier.Reset();
        this.MaxShieldModifier.Reset();
    }
}

[System.Serializable]
public struct WeaponModifiers
{
    public Modifier ProjectileSpeedModifier;
    public Modifier ProjectileSizeModifier;
    public Modifier ProjectileLifetimeModifier;
    public Modifier CooldownModifier;
    public Modifier DamageModifier;

    public void Add(WeaponModifiers other)
    {
        this.ProjectileSpeedModifier.Add(other.ProjectileSpeedModifier);
        this.ProjectileSizeModifier.Add(other.ProjectileSizeModifier);
        this.CooldownModifier.Add(other.CooldownModifier);
        this.DamageModifier.Add(other.DamageModifier);
        this.ProjectileLifetimeModifier.Add(other.ProjectileLifetimeModifier);
    }

    public void Scale(int scale)
    {
        this.ProjectileSizeModifier.Scale(scale);
        this.ProjectileSpeedModifier.Scale(scale);
        this.CooldownModifier.Scale(scale);
        this.DamageModifier.Scale(scale);
        this.ProjectileLifetimeModifier.Scale(scale);
    }

    public void Reset()
    {
        this.ProjectileSizeModifier.Reset();
        this.ProjectileSpeedModifier.Reset();
        this.CooldownModifier.Reset();
        this.DamageModifier.Reset();
        this.ProjectileLifetimeModifier.Reset();
    }
}

[System.Serializable]
public struct Modifier
{
    [Tooltip("Absolute amount added or removed")]
    public float AdditiveMod;

    [Tooltip("Amount added or removed in %")]
    public float MultiplyMod;

    public void Add(Modifier other)
    {
        this.AdditiveMod += other.AdditiveMod;
        this.MultiplyMod += other.MultiplyMod;
    }

    public void Scale(int scale)
    {
        this.AdditiveMod *= scale;
        this.MultiplyMod *= scale;
    }

    public void Reset()
    {
        this.AdditiveMod = 0f;
        this.MultiplyMod = 0f;
    }
}

public struct ActiveModifier : IBufferElementData
{
    public int ID;
    public int Count;
    public PlayerStatsModifiers StatMods;
    public float Duration;
}
