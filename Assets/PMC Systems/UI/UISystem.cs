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
            var targetID = addUpgradeEvent.ValueRO.PlayerID;
            var upgradeData = addUpgradeEvent.ValueRO.UpgradeToAdd;
            Entity targetPlayer = Entity.Null;

            foreach (var (index, entity) in SystemAPI.Query<RefRO<Player>>().WithEntityAccess())
            {
                if (index.ValueRO.PlayerID == targetID)
                {
                    targetPlayer = entity;
                    break;
                }
            }

            if (targetPlayer != Entity.Null)
            {
                var playerModsBuffer = SystemAPI.GetBuffer<ActiveModifier>(targetPlayer);
                ModifierUtils.AddModifier(playerModsBuffer, upgradeData);
            }
            else
            {
                Debug.LogWarning($"Could not find Player with ID: {targetID}");
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
    public int PlayerID;
}


public enum InterfaceState
{
    TestScreen1,
    TestScreen2,
    GameStartScreen,
    Hidden,
}
