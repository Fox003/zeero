using Unity.Entities;

public struct UIScreens : IComponentData
{
    public UnityObjectRef<GameCountdownScreen> GameCountdownScreen;
    public UnityObjectRef<GameFightingScreen> GameFightingScreen;
    public UnityObjectRef<UpgradePhaseScreen> GameUpgradePhaseScreen;
    public UnityObjectRef<WaitingForPlayersScreen> WaitingForPlayersScreen;
}
