using Unity.Entities;
using UnityEngine;

class UIFSMAuthoring : MonoBehaviour
{
    
}

class UIFSMBaker : Baker<UIFSMAuthoring>
{
    public override void Bake(UIFSMAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        
        AddComponent<UIFSM>(entity);
        AddComponent(entity, new CurrentStateType(){Type = UIFSMStates.GAME_WAITING_FOR_PLAYERS});
        
        
        AddComponent<UIStateFighting>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_FIGHTING_STATE, false);
        
        AddComponent<UIStateHidden>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.HIDDEN_STATE, false);
        
        AddComponent<UIStateGameOver>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_GAMEOVER_STATE, false);

        AddComponent<UIStateCountdown>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_COUNTDOWN_STATE, false);

        AddComponent<UIStateUpgradePhase>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_UPGRADE_PHASE_STATE, false);

        AddComponent<UIStateWaitingForPlayers>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_WAITING_FOR_PLAYERS, true);

    }
    
}