using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

public struct GameData : IComponentData
{
    public PlayerData Players;
}

public struct InGameState : IComponentData, IEnableableComponent {}

public struct PlayerData : IBufferElementData
{
    
}

public struct GameTimerData : IComponentData
{
    public bool IsPaused;
    public bool IsDone;
    public float MaxTime;
    public float CurrentTime;

    public void Reset()
    {
        IsPaused = true;
        IsDone = false;
        CurrentTime = MaxTime;
    }
}

public struct GameConditions : IComponentData
{
    public bool IsPlayerDead;
    public bool IsTimeUp;
    public bool start;
    
    public void Reset()
    {
        IsTimeUp = false;
        IsPlayerDead = false;
    }
}