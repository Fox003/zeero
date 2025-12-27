using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(CommitStateTransitionSystem))]
[UpdateBefore(typeof(UIStateTransitions))]
partial struct GameStateTransitions : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIFSM>();
        state.RequireForUpdate<GameFSM>();
        state.RequireForUpdate<GameTimerData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (fsm, entity) in SystemAPI.Query<RefRO<GameFSM>>()
                     .WithChangeFilter<CurrentStateType>()
                     .WithEntityAccess())
        {
            if (SystemAPI.IsComponentEnabled<GameStateInit>(entity))
            {
                var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
                var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
                var gameData = SystemAPI.GetSingleton<GameData>();
                var upgradePickerEntity = SystemAPI.GetSingletonEntity<CurrentUpgrades>();

                foreach (var (player, movement, weapon, health, stats, playerEntity) in
                         SystemAPI.Query<Player, RefRW<MovementState>, RefRW<WeaponState>, RefRW<HealthState>, PlayerBaseStats>()
                         .WithEntityAccess())
                {
                    float3 startPos = (player.PlayerID == 2) ? gameData.player1StartPos : gameData.player2StartPos;
                    SetPlayerPosition(playerEntity, startPos, ref state);

                    movement.ValueRW.Reset();
                    weapon.ValueRW.Reset();
                    health.ValueRW.Reset(stats);

                    SystemAPI.SetComponentEnabled<RequestUpgradeRollTag>(upgradePickerEntity, true);
                }

                GameFSMUtilities.OnInitChangeGameState(
                    uiFsm,
                    gameFSM,
                    SystemAPI.GetBuffer<EnableStateRequest>(uiFsm),
                    SystemAPI.GetBuffer<EnableStateRequest>(gameFSM)
                );
            }
            // OnEnter GameStateFighting
            else if (SystemAPI.IsComponentEnabled<GameStateFighting>(entity))
            {
                // Start the timer
                var timerData = SystemAPI.GetSingletonRW<GameTimerData>();

                GameFSMUtilities.OnFightingStateEnter(ref timerData.ValueRW);
            }
        }
    }

    private void SetPlayerPosition(Entity player, float3 position, ref SystemState state)
    {
        var transform = SystemAPI.GetComponent<LocalTransform>(player);

        transform.Position = position;

        SystemAPI.SetComponent(player, transform);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
