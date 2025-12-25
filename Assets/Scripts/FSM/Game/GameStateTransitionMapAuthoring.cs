using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class GameStateTransitionMapAuthoring : MonoBehaviour
{
    
}

class GameStateTransitionMapAuthoringBaker : Baker<GameStateTransitionMapAuthoring>
{
    public override void Bake(GameStateTransitionMapAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        
        var transitionPairs = new List<TransitionPair>
        {
            new TransitionPair(GameFSMStates.INIT_STATE, GameFSMStates.COUNTDOWN_STATE),
            new TransitionPair(GameFSMStates.COUNTDOWN_STATE, GameFSMStates.FIGHTING_STATE),
            new TransitionPair(GameFSMStates.FIGHTING_STATE, GameFSMStates.ROUND_END_STATE),
            new TransitionPair(GameFSMStates.FIGHTING_STATE, GameFSMStates.UPGRADE_PHASE_STATE),
            new TransitionPair(GameFSMStates.ROUND_END_STATE, GameFSMStates.UPGRADE_PHASE_STATE),
            new TransitionPair(GameFSMStates.ROUND_END_STATE, GameFSMStates.MATCH_END_STATE),
            new TransitionPair(GameFSMStates.UPGRADE_PHASE_STATE, GameFSMStates.INIT_STATE),
        };
        
        AddComponent(entity, new GameStateTransitionMap()
        {
            Transitions = BlobUtils.CreateBlobArrayRefFromList(transitionPairs)
        });
    }
}
