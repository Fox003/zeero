using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ValidGameStateTransitions", menuName = "FSM SOs/Game/ValidGameStateTransitions")]
public class ValidGameStateTransitions : ScriptableObject
{
    public List<GameStateTransition> transitions;
}

[System.Serializable]
public struct GameStateTransition
{
    public GameFSMStates.GameState FromState;
    public List<GameFSMStates.GameState> ToStates;
}