using Unity.Entities;
using UnityEngine;

public static class UIFSMStates
{
    public static readonly ComponentType HIDDEN_STATE = ComponentType.ReadOnly<UIStateHidden>();
    public static readonly ComponentType GAME_STARTING_STATE = ComponentType.ReadOnly<UIStateStarting>();
    public static readonly ComponentType GAME_FIGHTING_STATE = ComponentType.ReadOnly<UIStateFighting>();
    public static readonly ComponentType GAME_GAMEOVER_STATE = ComponentType.ReadOnly<UIStateGameOver>();
}

public struct UIStateHidden : IComponentData, IEnableableComponent {}
public struct UIStateFighting : IComponentData, IEnableableComponent {}
public struct UIStateGameOver : IComponentData, IEnableableComponent {}
public struct UIStateStarting : IComponentData, IEnableableComponent {}

public struct UIFSM : IComponentData {}