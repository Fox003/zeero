using Unity.Entities;
using UnityEngine;

public static class UIFSMStates
{
    public static readonly ComponentType HIDDEN_STATE = ComponentType.ReadOnly<UIStateHidden>();
    public static readonly ComponentType GAME_COUNTDOWN_STATE = ComponentType.ReadOnly<UIStateCountdown>();
    public static readonly ComponentType GAME_FIGHTING_STATE = ComponentType.ReadOnly<UIStateFighting>();
    public static readonly ComponentType GAME_GAMEOVER_STATE = ComponentType.ReadOnly<UIStateGameOver>();
    public static readonly ComponentType GAME_UPGRADE_PHASE_STATE = ComponentType.ReadOnly<UIStateUpgradePhase>();

    public enum UIState
    {
        HIDDEN_STATE,
        GAME_COUNTDOWN_STATE,
        GAME_FIGHTING_STATE, 
        GAME_GAMEOVER_STATE,
        GAME_UPGRADE_PHASE_STATE
    }

    public static ComponentType Resolve(UIState state)
    {
        return state switch
        {
            UIState.HIDDEN_STATE => HIDDEN_STATE,
            UIState.GAME_COUNTDOWN_STATE => GAME_COUNTDOWN_STATE,
            UIState.GAME_FIGHTING_STATE => GAME_FIGHTING_STATE,
            UIState.GAME_GAMEOVER_STATE => GAME_GAMEOVER_STATE,
            UIState.GAME_UPGRADE_PHASE_STATE => GAME_UPGRADE_PHASE_STATE,
        };
    }
}

public struct UIStateHidden : IComponentData, IEnableableComponent {}
public struct UIStateFighting : IComponentData, IEnableableComponent {}
public struct UIStateGameOver : IComponentData, IEnableableComponent {}
public struct UIStateUpgradePhase : IComponentData, IEnableableComponent { }
public struct UIStateCountdown : IComponentData, IEnableableComponent {}

public struct UIFSM : IComponentData {}