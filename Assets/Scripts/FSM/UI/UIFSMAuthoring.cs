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
        AddComponent(entity, new CurrentStateType(){Type = UIFSMStates.HIDDEN_STATE});
        
        
        AddComponent<UIStateFighting>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_FIGHTING_STATE, false);
        
        AddComponent<UIStateHidden>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.HIDDEN_STATE, false);
        
        AddComponent<UIStateGameOver>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_GAMEOVER_STATE, false);

        AddComponent<UIStateStarting>(entity);
        FSMUtilities.SetComponentStateReflectively(this, entity, UIFSMStates.GAME_STARTING_STATE, false);

    }
    
}