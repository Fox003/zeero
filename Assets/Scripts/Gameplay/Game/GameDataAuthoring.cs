using Unity.Entities;
using UnityEngine;

class GameDataAuthoring : MonoBehaviour
{
    public float MaxGameTime;
    public GameState InitialGameState;
}

class GameDataAuthoringBaker : Baker<GameDataAuthoring>
{
    public override void Bake(GameDataAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        
        AddComponent(entity, new GameTimerData()
        {
            CurrentTime = authoring.MaxGameTime,
            MaxTime = authoring.MaxGameTime,
            IsPaused = true,
        });
        
        AddComponent(entity, new GameConditions()
        {
            IsPlayerDead = false,
            IsTimeUp = false
        });
        
        AddComponent(entity, new GameData()
        {
            Players = new PlayerData()
        });
        
        AddComponent<GameManager>(entity);
    }
}

public struct GameManager : IComponentData {}


