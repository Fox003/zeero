using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class GameStateTransitionMapAuthoring : MonoBehaviour
{
    public ValidGameStateTransitions validTransitions;
}

class GameStateTransitionMapAuthoringBaker : Baker<GameStateTransitionMapAuthoring>
{
    public override void Bake(GameStateTransitionMapAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        var transitionPairs = new List<TransitionPair>();

        foreach (var entry in authoring.validTransitions.transitions)
        {
            foreach (var targetState in entry.ToStates)
            {
                transitionPairs.Add(new TransitionPair(
                    from: GameFSMStates.Resolve(entry.FromState),
                    to: GameFSMStates.Resolve(targetState)
                ));
            }
        }

        AddComponent(entity, new GameStateTransitionMap()
        {
            Transitions = BlobUtils.CreateBlobArrayRefFromList(transitionPairs)
        });
    }
}
