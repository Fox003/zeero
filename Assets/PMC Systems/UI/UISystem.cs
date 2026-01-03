using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

partial struct UISystem : ISystem
{
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIFSM>();
        state.RequireForUpdate<GameFSM>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var gameFsm = SystemAPI.GetSingletonEntity<GameFSM>();
        var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();

        // UI State change event
        foreach (var (stateChangeEvent, eventTag) in SystemAPI.Query<RefRO<UIStateChangeEvent>, RefRO<UIEvent>>())
        {
            var gameAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(gameFsm);
            var uiAddBuffer = SystemAPI.GetBuffer<EnableStateRequest>(uiFsm);
            
            gameAddBuffer.Add(new EnableStateRequest()
            {
                Entity = gameFsm,
                IgnoreRequestFlag = false,
                StateToEnable = stateChangeEvent.ValueRO.NewGameState,
            });
            
            uiAddBuffer.Add(new EnableStateRequest()
            {
                Entity = uiFsm,
                IgnoreRequestFlag = false,
                StateToEnable = stateChangeEvent.ValueRO.NewUIState
            });
        }

        // Add upgrade to player event
        foreach (var (addUpgradeEvent, eventTag) in SystemAPI.Query<RefRO<AddUpgradeToPlayerEvent>, RefRO<UIEvent>>())
        {
            var target = addUpgradeEvent.ValueRO.Player;
            var upgradeData = addUpgradeEvent.ValueRO.UpgradeToAdd;

            if (target != Entity.Null)
            {
                var playerModsBuffer = SystemAPI.GetBuffer<ActiveModifier>(target);
                ModifierUtils.AddModifier(playerModsBuffer, upgradeData);

                var playerRanks = SystemAPI.GetSingletonBuffer<PlayerRoundRank>();
                if (!playerRanks.IsEmpty)
                {
                    playerRanks.RemoveAt(0);
                }
            }
            else
            {
                Debug.LogWarning($"Not player entity was passed through the event");
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public struct UIStateChangeEvent : IComponentData
{
    public ComponentType NewGameState;
    public ComponentType NewUIState;
}

public struct AddUpgradeToPlayerEvent : IComponentData 
{
    public UpgradeDefinition UpgradeToAdd;
    public Entity Player;
}


public enum InterfaceState
{
    TestScreen1,
    TestScreen2,
    GameStartScreen,
    Hidden,
}
