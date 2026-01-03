using Unity.Burst;
using Unity.Entities;

partial struct PlayerReadyUpSystem : ISystem
{
    private EntityQuery _readyPlayerQuery;

    public void OnCreate(ref SystemState state)
    {
        _readyPlayerQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<Player>(),
            ComponentType.Exclude<PlayerNeedsInputAssociation>()
        );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        int readyPlayers = _readyPlayerQuery.CalculateEntityCount();

        if (readyPlayers >= 2)
        {
            var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
            var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
            var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFSM);
            var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);

            FSMUtilities.ChangeFSMState(gameFSM, gameAddBuffer, GameFSMStates.INIT_STATE);
            FSMUtilities.ChangeFSMState(uiFsm, uiAddBuffer, UIFSMStates.HIDDEN_STATE);

            state.Enabled = false;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
