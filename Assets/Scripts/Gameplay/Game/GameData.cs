using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Mathematics;

public struct GameData : IComponentData
{
    public int PointsToWin;
    public float3 player1StartPos;
    public float3 player2StartPos;
}

public struct InGameState : IComponentData, IEnableableComponent {}

public struct PlayerData : IBufferElementData
{
    
}

public struct LevelRegistry : IComponentData
{
    public BlobAssetReference<BlobArray<EntitySceneReference>> LevelsBlob;
}

public struct CurrentLevelState : IComponentData
{
    public Entity Level;
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