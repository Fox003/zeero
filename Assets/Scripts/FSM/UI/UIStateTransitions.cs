using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(CommitStateTransitionSystem))]
partial struct UIStateTransitions : ISystem
{
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UIFSM>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var screens = SystemAPI.GetSingletonRW<UIScreens>();
        var uiFsm = SystemAPI.GetSingletonEntity<UIFSM>();
        
        foreach (var (fsm, entity) in SystemAPI.Query<RefRO<UIFSM>>()
                     .WithChangeFilter<CurrentStateType>()
                     .WithEntityAccess())
        {
            if (SystemAPI.IsComponentEnabled<UIStateCountdown>(entity))
            {
                ShowStartingStateScreens(ref screens.ValueRW);
            }
            else if (SystemAPI.IsComponentEnabled<UIStateHidden>(entity))
            {
                HideAllScreens(ref screens.ValueRW);
            }
            else if (SystemAPI.IsComponentEnabled<UIStateFighting>(entity))
            {
                var fightingViewModel = SystemAPI.ManagedAPI.GetSingleton<GameFightingViewModel>();

                ShowFightingStateScreens(ref screens.ValueRW);
                screens.ValueRW.GameFightingScreen.Value.BindData(fightingViewModel);
            }
            else if (SystemAPI.IsComponentEnabled<UIStateUpgradePhase>(entity))
            {
                var upgradesViewModel = SystemAPI.ManagedAPI.GetSingleton<GameUpgradesViewModel>();

                screens.ValueRW.GameUpgradePhaseScreen.Value.BindData(upgradesViewModel);
                ShowUpgradePhaseScreens(ref screens.ValueRW);
            }
        }
    }

    private void HideAllScreens(ref UIScreens screens)
    {
        screens.GameCountdownScreen.Value.Hide();
        screens.GameFightingScreen.Value.Hide();
        screens.GameUpgradePhaseScreen.Value.Hide();
    }

    private void ShowStartingStateScreens(ref UIScreens screens)
    {
        screens.GameCountdownScreen.Value.Show();
        screens.GameFightingScreen.Value.Hide();
        screens.GameUpgradePhaseScreen.Value.Hide();
    }
    
    private void ShowFightingStateScreens(ref UIScreens screens)
    {
        screens.GameCountdownScreen.Value.Hide();
        screens.GameFightingScreen.Value.Show();
        screens.GameUpgradePhaseScreen.Value.Hide();
    }

    private void ShowUpgradePhaseScreens(ref UIScreens screens)
    {
        screens.GameCountdownScreen.Value.Hide();
        screens.GameFightingScreen.Value.Hide();
        screens.GameUpgradePhaseScreen.Value.Show();
    }
}
