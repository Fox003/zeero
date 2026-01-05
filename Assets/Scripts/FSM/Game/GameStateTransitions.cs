using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

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

                LoadNewRandomLevel(ref ecb, ref state);

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
                var gameData = SystemAPI.GetSingleton<GameData>();

                

                var timerData = SystemAPI.GetSingletonRW<GameTimerData>();

                timerData.ValueRW.IsPaused = true;
                timerData.ValueRW.Reset();

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

                
                AddPointToPlayer(playerList.ElementAt(0).Entity, ref state);

                if (HasAnyPlayerWon(playerList, gameData.PointsToWin, ref state))
                {
                    FSMUtilities.ChangeFSMState(gameFSM, state.EntityManager, GameFSMStates.MATCH_END_STATE);
                    FSMUtilities.ChangeFSMState(uiFsm, state.EntityManager, UIFSMStates.GAME_GAMEOVER_STATE);

                    return;
                }
                
                DeterminePlayerRanks(playerList, ref state);
                

                FSMUtilities.ChangeFSMState(gameFSM, state.EntityManager, GameFSMStates.UPGRADE_PHASE_STATE);
                FSMUtilities.ChangeFSMState(uiFsm, state.EntityManager, UIFSMStates.GAME_UPGRADE_PHASE_STATE);
            }
            // OnEnter GameStateMatchEnd
            else if (SystemAPI.IsComponentEnabled<GameStateMatchEnd>(entity))
            {
                foreach (var (playerTag, playerGameStats) in SystemAPI.Query<RefRO<Player>, RefRW<GameStats>>())
                {
                    playerGameStats.ValueRW.CurrentPoints = 0;
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
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

    private void DeterminePlayerRanks(NativeList<PlayerSortWrapper> playerList, ref SystemState state)
    {
        var gameDataEntity = SystemAPI.GetSingletonEntity<GameManager>();
        var buffer = SystemAPI.GetBuffer<PlayerRoundRank>(gameDataEntity);
        buffer.Clear();

        foreach (var playerEntry in playerList)
        {
            buffer.Add(new PlayerRoundRank { Player = playerEntry.Entity });
        }
    }

    private bool HasAnyPlayerWon(NativeList<PlayerSortWrapper> playerList, int pointsToWin, ref SystemState state)
    {
        foreach (var player in playerList)
        {
            if (SystemAPI.GetComponent<GameStats>(player.Entity).CurrentPoints >= pointsToWin)
            {
                return true;
            }
        }

        return false;
    }

    private void AddPointToPlayer(Entity player, ref SystemState state)
    {
        var playerGameStats = SystemAPI.GetComponentRW<GameStats>(player);
        playerGameStats.ValueRW.CurrentPoints++;
    }

    private void LoadNewRandomLevel(ref EntityCommandBuffer ecb, ref SystemState state)
    {
        var registry = SystemAPI.GetSingleton<LevelRegistry>();
        uint baseSeed = (uint)UnityEngine.Random.Range(1, uint.MaxValue);

        var rng = Unity.Mathematics.Random.CreateFromIndex(baseSeed + ((uint)SystemAPI.Time.ElapsedTime * 1000));

        int randomIndex = rng.NextInt(0, registry.LevelsBlob.Value.Length);
        var nextSceneRef = registry.LevelsBlob.Value[randomIndex];

        var levelState = SystemAPI.TryGetSingleton<CurrentLevelState>(out var currentLevel);

        if (currentLevel.Level != Entity.Null)
        {
            var unloadReq = ecb.CreateEntity();
            ecb.AddComponent(unloadReq, new SceneUnloadRequest { SceneEntity = currentLevel.Level });
        }

        var loadReq = ecb.CreateEntity();
        ecb.AddComponent(loadReq, new SceneLoadRequest { SceneReference = nextSceneRef });
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
