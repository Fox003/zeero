using Unity.Entities;
using UnityEngine;

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
}

public struct GameStateInit : IComponentData, IEnableableComponent {}
public struct GameStateCountdown : IComponentData, IEnableableComponent {}
public struct GameStateFighting : IComponentData, IEnableableComponent {}
public struct GameStateUpgradePhase : IComponentData, IEnableableComponent {}
public struct GameStateRoundEnd : IComponentData, IEnableableComponent {}
public struct GameStateMatchEnd : IComponentData, IEnableableComponent {}

public struct GameFSM : IComponentData {}