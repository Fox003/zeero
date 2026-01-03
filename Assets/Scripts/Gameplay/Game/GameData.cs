using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct GameData : IComponentData
{
    public float3 player1StartPos;
    public float3 player2StartPos;
}

public struct InGameState : IComponentData, IEnableableComponent {}

public struct PlayerData : IBufferElementData
{
    
}

public struct PlayerRoundRank : IBufferElementData
{
    public Entity Player;
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