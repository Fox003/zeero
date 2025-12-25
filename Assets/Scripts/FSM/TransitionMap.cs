using System;
using Unity.Entities;

public readonly struct TransitionPair
{
    public readonly ComponentType FromState;
    public readonly ComponentType ToState;

    public TransitionPair(ComponentType from, ComponentType to)
    {
        FromState = from;
        ToState = to;
    }
}