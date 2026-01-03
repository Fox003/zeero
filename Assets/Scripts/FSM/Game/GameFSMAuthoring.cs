using Unity.Entities;
using UnityEngine;

class GameFSMAuthoring : MonoBehaviour
{
}

class GameFSMAuthoringBaker : Baker<GameFSMAuthoring>
{
    public override void Bake(GameFSMAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        
        AddComponent<GameFSM>(entity);
        AddComponent(entity, new CurrentStateType(){Type = GameFSMStates.WAITING_FOR_PLAYERS_STATE});

        AddComponent(entity, GameFSMStates.INIT_STATE);
        FSMUtilities.SetComponentStateReflectively(this, entity, GameFSMStates.INIT_STATE, false);

        AddComponent(entity, GameFSMStates.COUNTDOWN_STATE);
        FSMUtilities.SetComponentStateReflectively(this, entity, GameFSMStates.COUNTDOWN_STATE, false);
        
        AddComponent(entity, GameFSMStates.FIGHTING_STATE);
        FSMUtilities.SetComponentStateReflectively(this, entity, GameFSMStates.FIGHTING_STATE, false);
        
        AddComponent(entity, GameFSMStates.MATCH_END_STATE);
        FSMUtilities.SetComponentStateReflectively(this, entity, GameFSMStates.MATCH_END_STATE, false);
        
        AddComponent(entity, GameFSMStates.ROUND_END_STATE);
        FSMUtilities.SetComponentStateReflectively(this, entity, GameFSMStates.ROUND_END_STATE, false);
        
        AddComponent(entity, GameFSMStates.UPGRADE_PHASE_STATE);
        FSMUtilities.SetComponentStateReflectively(this, entity, GameFSMStates.UPGRADE_PHASE_STATE, false);

        AddComponent(entity, GameFSMStates.WAITING_FOR_PLAYERS_STATE);
        FSMUtilities.SetComponentStateReflectively(this, entity, GameFSMStates.WAITING_FOR_PLAYERS_STATE, true);
    }
}

