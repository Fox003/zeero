using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class UITransitionMapAuthoring : MonoBehaviour
{
    public ValidUIStateTransitions validTransitions;
}

class UITransitionMapAuthoringBaker : Baker<UITransitionMapAuthoring>
{
    public override void Bake(UITransitionMapAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);


        var transitionPairs = new List<TransitionPair>();

        foreach (var entry in authoring.validTransitions.transitions)
        {
            foreach (var targetState in entry.ToStates)
            {
                transitionPairs.Add(new TransitionPair(
                    from: UIFSMStates.Resolve(entry.FromState),
                    to: UIFSMStates.Resolve(targetState)
                ));
            }
        }

        AddComponent(entity, new UIStateTransitionMap()
        {
            Transitions = BlobUtils.CreateBlobArrayRefFromList(transitionPairs)
        });
    }
}
