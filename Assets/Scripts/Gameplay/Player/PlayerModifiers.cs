using Unity.Entities;

[System.Serializable]
public struct PlayerStatsModifiers : IComponentData
{
    public MovementModifiers MovementStatsModifiers;
    public HealthModifiers HealthStatsModifiers;

    public void Reset()
    {
        this.MovementStatsModifiers.Reset();
        this.HealthStatsModifiers.Reset();
    }

    public void Add(PlayerStatsModifiers other)
    {
        this.MovementStatsModifiers.Add(other.MovementStatsModifiers);
        this.HealthStatsModifiers.Add(other.HealthStatsModifiers);
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

    public void Reset()
    {
        this.MaxHealthModifier.Reset();
        this.MaxShieldModifier.Reset();
    }
}

[System.Serializable]
public struct Modifier
{
    public float AdditiveMod;
    public float MultiplyMod;

    public void Add(Modifier other)
    {
        this.AdditiveMod += other.AdditiveMod;
        this.MultiplyMod += other.MultiplyMod;
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
    public PlayerStatsModifiers StatMods;
    public float Duration;
}
