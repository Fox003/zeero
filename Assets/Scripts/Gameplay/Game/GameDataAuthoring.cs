using Unity.Entities;
using UnityEngine;

class GameDataAuthoring : MonoBehaviour
{
    public float MaxGameTime;
    public Transform Player1StartTransform;
    public Transform Player2StartTransform;
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
            player1StartPos = authoring.Player1StartTransform.position,
            player2StartPos = authoring.Player2StartTransform.position,
        });
        
        AddComponent<GameManager>(entity);
    }
}

public struct GameManager : IComponentData {}


