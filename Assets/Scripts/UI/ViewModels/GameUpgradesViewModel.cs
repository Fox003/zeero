using Unity.Entities;

public class GameUpgradesViewModel : IComponentData
{
    public int CurrentUpgradingPlayerID;

    public UpgradeDefinition Upgrade1;
    public UpgradeDefinition Upgrade2;
    public UpgradeDefinition Upgrade3;
}
