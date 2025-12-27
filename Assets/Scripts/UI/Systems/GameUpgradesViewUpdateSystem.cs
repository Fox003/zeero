using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct GameUpgradesViewUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var viewModel = SystemAPI.ManagedAPI.GetSingleton<GameUpgradesViewModel>();
        var currentPlayerUpgrading = SystemAPI.GetSingleton<CurrentPlayerUpgrading>();

        foreach (var currentUpgrades in SystemAPI.Query<RefRO<CurrentUpgrades>>().WithChangeFilter<CurrentUpgrades>())
        {
            var upgradeDB = SystemAPI.GetSingleton<UpgradeLibraryBlob>();

            viewModel.Upgrade1 = upgradeDB.AllUpgrades.Value[currentUpgrades.ValueRO.UpgradeID1];
            viewModel.Upgrade2 = upgradeDB.AllUpgrades.Value[currentUpgrades.ValueRO.UpgradeID2];
            viewModel.Upgrade3 = upgradeDB.AllUpgrades.Value[currentUpgrades.ValueRO.UpgradeID3];
        }

        viewModel.CurrentUpgradingPlayerID = currentPlayerUpgrading.PlayerID;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
