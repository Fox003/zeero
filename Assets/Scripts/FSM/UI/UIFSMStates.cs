using Unity.Entities;
using UnityEngine;

public static class UIFSMStates
{
    public static readonly ComponentType HIDDEN_STATE = ComponentType.ReadOnly<UIStateHidden>();
    public static readonly ComponentType GAME_COUNTDOWN_STATE = ComponentType.ReadOnly<UIStateCountdown>();
    public static readonly ComponentType GAME_FIGHTING_STATE = ComponentType.ReadOnly<UIStateFighting>();
    public static readonly ComponentType GAME_GAMEOVER_STATE = ComponentType.ReadOnly<UIStateGameOver>();
    public static readonly ComponentType GAME_UPGRADE_PHASE_STATE = ComponentType.ReadOnly<UIStateUpgradePhase>();
}

public struct UIStateHidden : IComponentData, IEnableableComponent {}
public struct UIStateFighting : IComponentData, IEnableableComponent {}
public struct UIStateGameOver : IComponentData, IEnableableComponent {}
public struct UIStateUpgradePhase : IComponentData, IEnableableComponent { }
public struct UIStateCountdown : IComponentData, IEnableableComponent {}

public struct UIFSM : IComponentData {}