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
            new TransitionPair(from: UIFSMStates.GAME_COUNTDOWN_STATE, to: UIFSMStates.GAME_FIGHTING_STATE),
            new TransitionPair(from: UIFSMStates.GAME_FIGHTING_STATE, to: UIFSMStates.GAME_UPGRADE_PHASE_STATE),
            new TransitionPair(from: UIFSMStates.GAME_UPGRADE_PHASE_STATE, to: UIFSMStates.GAME_GAMEOVER_STATE),

            new TransitionPair(from: UIFSMStates.HIDDEN_STATE, to: UIFSMStates.GAME_COUNTDOWN_STATE),
            new TransitionPair(from: UIFSMStates.HIDDEN_STATE, to: UIFSMStates.GAME_GAMEOVER_STATE),
            new TransitionPair(from: UIFSMStates.HIDDEN_STATE, to: UIFSMStates.GAME_FIGHTING_STATE),
            new TransitionPair(from: UIFSMStates.HIDDEN_STATE, to: UIFSMStates.GAME_UPGRADE_PHASE_STATE),

            new TransitionPair(to: UIFSMStates.HIDDEN_STATE, from: UIFSMStates.GAME_COUNTDOWN_STATE),
            new TransitionPair(to: UIFSMStates.HIDDEN_STATE, from: UIFSMStates.GAME_GAMEOVER_STATE),
            new TransitionPair(to: UIFSMStates.HIDDEN_STATE, from: UIFSMStates.GAME_FIGHTING_STATE),
            new TransitionPair(to: UIFSMStates.HIDDEN_STATE, from: UIFSMStates.GAME_UPGRADE_PHASE_STATE),
        };
        
        AddComponent(entity, new UIStateTransitionMap()
        {
            Transitions = BlobUtils.CreateBlobArrayRefFromList(transitionPairs)
        });
    }
}
