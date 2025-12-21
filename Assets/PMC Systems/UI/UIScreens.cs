using Unity.Entities;

public struct UIScreens : IComponentData
{
    public UnityObjectRef<TestScreen1> TestScreen1;
    public UnityObjectRef<TestScreen2> TestScreen2;
    public UnityObjectRef<GameStartScreen> GameStartScreen;
    public UnityObjectRef<GameFightingScreen> GameFightingScreen;
    public UnityObjectRef<UIScreen> GameCountdownScreen;
}
