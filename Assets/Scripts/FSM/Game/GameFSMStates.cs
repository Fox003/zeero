using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public static class GameFSMStates
{
    // A voir si c'est utile comme state, j'ai fait un state par defaut dans le fsm base authoring
    // public static readonly ComponentType NONE_STATE = ComponentType.ReadOnly<GameStateFighting>();
    public static readonly ComponentType INIT_STATE = ComponentType.ReadOnly<GameStateInit>();
    public static readonly ComponentType FIGHTING_STATE = ComponentType.ReadOnly<GameStateFighting>();
    public static readonly ComponentType COUNTDOWN_STATE = ComponentType.ReadOnly<GameStateCountdown>();
    public static readonly ComponentType MATCH_END_STATE = ComponentType.ReadOnly<GameStateMatchEnd>();
    public static readonly ComponentType ROUND_END_STATE = ComponentType.ReadOnly<GameStateRoundEnd>();
    public static readonly ComponentType UPGRADE_PHASE_STATE = ComponentType.ReadOnly<GameStateUpgradePhase>();
    public static readonly ComponentType WAITING_FOR_PLAYERS_STATE = ComponentType.ReadOnly<GameStateWaitingForPlayers>();

    public enum GameState {
        INIT_STATE,
        FIGHTING_STATE,
        COUNTDOWN_STATE,
        MATCH_END_STATE,
        ROUND_END_STATE,
        UPGRADE_PHASE_STATE,
        WAITING_FOR_PLAYERS_STATE,
    }

    public static ComponentType Resolve(GameState state)
    {
        return state switch
        {
            GameState.INIT_STATE => INIT_STATE,
            GameState.FIGHTING_STATE => FIGHTING_STATE,
            GameState.COUNTDOWN_STATE => COUNTDOWN_STATE,
            GameState.MATCH_END_STATE => MATCH_END_STATE,
            GameState.ROUND_END_STATE => ROUND_END_STATE,
            GameState.UPGRADE_PHASE_STATE => UPGRADE_PHASE_STATE,
            GameState.WAITING_FOR_PLAYERS_STATE => WAITING_FOR_PLAYERS_STATE,
            _ => INIT_STATE
        };
    }
}

public struct GameStateWaitingForPlayers : IComponentData, IEnableableComponent { }
public struct GameStateInit : IComponentData, IEnableableComponent {}
public struct GameStateCountdown : IComponentData, IEnableableComponent {}
public struct GameStateFighting : IComponentData, IEnableableComponent {}
public struct GameStateUpgradePhase : IComponentData, IEnableableComponent {}
public struct GameStateRoundEnd : IComponentData, IEnableableComponent {}
public struct GameStateMatchEnd : IComponentData, IEnableableComponent {}

public struct GameFSM : IComponentData {}