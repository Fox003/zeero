using System;
using Unity.Burst;
using Unity.Collections;
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
            // OnEnter GameStateInit
            if (SystemAPI.IsComponentEnabled<GameStateInit>(entity))
            {
                var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
                var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
                var gameData = SystemAPI.GetSingleton<GameData>();
                var upgradePickerEntity = SystemAPI.GetSingletonEntity<CurrentUpgrades>();

                foreach (var (player, movementState, weaponState, healthState, stats, playerEntity) in
                         SystemAPI.Query<Player, RefRW<MovementState>, RefRW<WeaponState>, RefRW<HealthState>, PlayerBaseStats>()
                         .WithEntityAccess())
                {
                    float3 startPos = (player.PlayerID == 2) ? gameData.player1StartPos : gameData.player2StartPos;
                    SetPlayerPosition(playerEntity, startPos, ref state);

                    movementState.ValueRW.Reset();
                    weaponState.ValueRW.Reset();
                    healthState.ValueRW.Reset(stats);

                    SystemAPI.SetComponentEnabled<RequestUpgradeRollTag>(upgradePickerEntity, true);
                }

                FSMUtilities.ChangeFSMState(gameFSM, state.EntityManager, GameFSMStates.COUNTDOWN_STATE);
                FSMUtilities.ChangeFSMState(uiFsm, state.EntityManager, UIFSMStates.GAME_COUNTDOWN_STATE);
            }

            // OnEnter GameStateFighting
            else if (SystemAPI.IsComponentEnabled<GameStateFighting>(entity))
            {
                var timerData = SystemAPI.GetSingletonRW<GameTimerData>();

                timerData.ValueRW.IsPaused = false;
            }

            // OnEnter GameStateRoundEnd
            else if (SystemAPI.IsComponentEnabled<GameStateRoundEnd>(entity))
            {
                var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
                var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
                var timerData = SystemAPI.GetSingletonRW<GameTimerData>();

                timerData.ValueRW.IsPaused = true;
                timerData.ValueRW.Reset();

                DeterminePlayerRanks(ref state);

                FSMUtilities.ChangeFSMState(gameFSM, state.EntityManager, GameFSMStates.UPGRADE_PHASE_STATE);
                FSMUtilities.ChangeFSMState(uiFsm, state.EntityManager, UIFSMStates.GAME_UPGRADE_PHASE_STATE);
            }
        }
    }

    struct PlayerSortWrapper : IComparable<PlayerSortWrapper>
    {
        public Entity Entity;
        public float Health;

        public int CompareTo(PlayerSortWrapper other)
        {
            // Sort descending: highest health first
            return other.Health.CompareTo(this.Health);
        }
    }

    private void SetPlayerPosition(Entity player, float3 position, ref SystemState state)
    {
        var transform = SystemAPI.GetComponent<LocalTransform>(player);

        transform.Position = position;

        SystemAPI.SetComponent(player, transform);
    }

    private void DeterminePlayerRanks(ref SystemState state)
    {
        var playerList = new NativeList<PlayerSortWrapper>(Allocator.Temp);

        foreach (var (health, playerEntity) in SystemAPI.Query<RefRO<HealthState>>().WithEntityAccess())
        {
            playerList.Add(new PlayerSortWrapper
            {
                Entity = playerEntity,
                Health = health.ValueRO.CurrentHealth
            });
        }

        playerList.Sort();

        var gameDataEntity = SystemAPI.GetSingletonEntity<GameManager>();
        var buffer = SystemAPI.GetBuffer<PlayerRoundRank>(gameDataEntity);
        buffer.Clear();

        foreach (var playerEntry in playerList)
        {
            buffer.Add(new PlayerRoundRank { Player = playerEntry.Entity });
        }

        playerList.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
