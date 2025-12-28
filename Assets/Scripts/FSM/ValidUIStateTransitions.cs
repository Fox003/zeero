using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ValidUIStateTransitions", menuName = "FSM SOs/UI/ValidUIStateTransitions")]
public class ValidUIStateTransitions : ScriptableObject
{
    public List<UIStateTransition> transitions;
}
[System.Serializable]
public struct UIStateTransition
{
    public UIFSMStates.UIState FromState;
    public List<UIFSMStates.UIState> ToStates;
}