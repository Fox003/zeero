using Unity.Burst;
using Unity.Entities;

partial struct UpgradesAppliedCheckSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
        var gameState = SystemAPI.GetComponent<CurrentStateType>(gameFSM);

        if (gameState.Type != GameFSMStates.UPGRADE_PHASE_STATE)
            return;

        var playerRanks = SystemAPI.GetSingletonBuffer<PlayerRoundRank>();
        
        if (playerRanks.IsEmpty)
        {
            var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();

            /*var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFSM);
            var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);

            gameAddBuffer.Add(new EnableStateRequest()
            {
                Entity = gameFSM,
                IgnoreRequestFlag = false,
                StateToEnable = GameFSMStates.INIT_STATE,
            });

            uiAddBuffer.Add(new EnableStateRequest()
            {
                Entity = uiFsm,
                IgnoreRequestFlag = false,
                StateToEnable = UIFSMStates.HIDDEN_STATE
            });*/

            FSMUtilities.ChangeFSMState(gameFSM, state.EntityManager, GameFSMStates.INIT_STATE);
            FSMUtilities.ChangeFSMState(uiFsm, state.EntityManager, UIFSMStates.HIDDEN_STATE);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
