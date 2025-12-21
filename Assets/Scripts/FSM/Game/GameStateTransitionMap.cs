using Unity.Entities;

public struct GameStateTransitionMap : IComponentData
{
    public BlobAssetReference<TransitionMapBlob> Transitions;
    
}
