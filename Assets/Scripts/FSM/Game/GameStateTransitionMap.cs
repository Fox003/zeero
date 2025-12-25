using Unity.Entities;

public struct GameStateTransitionMap : IComponentData
{
    public BlobAssetReference<BlobArray<TransitionPair>> Transitions;
    
}
