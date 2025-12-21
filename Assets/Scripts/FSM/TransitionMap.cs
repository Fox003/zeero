using System;
using Unity.Entities;

public readonly struct TransitionPair : IEquatable<TransitionPair>
{
    public readonly ComponentType FromState;
    public readonly ComponentType ToState;

    public TransitionPair(ComponentType from, ComponentType to)
    {
        FromState = from;
        ToState = to;
    }

    public bool Equals(TransitionPair other)
    {
        return FromState.TypeIndex == other.FromState.TypeIndex &&
               ToState.TypeIndex == other.ToState.TypeIndex;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(FromState.TypeIndex, ToState.TypeIndex);
    }
}

public struct TransitionMapBlob
{
    public BlobArray<TransitionPair> Pairs;
}
