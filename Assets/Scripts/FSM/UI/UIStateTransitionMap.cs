using System;
using Unity.Collections;
using Unity.Entities;

public struct UIStateTransitionMap : IComponentData
{
    public BlobAssetReference<BlobArray<TransitionPair>> Transitions;
}


