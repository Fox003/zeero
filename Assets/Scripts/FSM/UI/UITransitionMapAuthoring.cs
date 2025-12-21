using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class UITransitionMapAuthoring : MonoBehaviour
{
    
}

class UITransitionMapAuthoringBaker : Baker<UITransitionMapAuthoring>
{
    public override void Bake(UITransitionMapAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        
        
        var transitionPairs = new List<TransitionPair>
        {
            new TransitionPair(from: UIFSMStates.GAME_STARTING_STATE, to: UIFSMStates.GAME_FIGHTING_STATE),
            new TransitionPair(from: UIFSMStates.GAME_FIGHTING_STATE, to: UIFSMStates.GAME_GAMEOVER_STATE),
            new TransitionPair(from: UIFSMStates.GAME_GAMEOVER_STATE, to: UIFSMStates.GAME_STARTING_STATE),
            new TransitionPair(from: UIFSMStates.HIDDEN_STATE, to: UIFSMStates.GAME_STARTING_STATE),
            new TransitionPair(from: UIFSMStates.HIDDEN_STATE, to: UIFSMStates.GAME_GAMEOVER_STATE),
            new TransitionPair(from: UIFSMStates.HIDDEN_STATE, to: UIFSMStates.GAME_FIGHTING_STATE),
            new TransitionPair(to: UIFSMStates.HIDDEN_STATE, from: UIFSMStates.GAME_STARTING_STATE),
            new TransitionPair(to: UIFSMStates.HIDDEN_STATE, from: UIFSMStates.GAME_GAMEOVER_STATE),
            new TransitionPair(to: UIFSMStates.HIDDEN_STATE, from: UIFSMStates.GAME_FIGHTING_STATE)
        };
        
        AddComponent(entity, new UIStateTransitionMap()
        {
            Transitions = FSMUtilities.CreateTransitionMapBlobAssetReference(transitionPairs)
        });
    }
}
