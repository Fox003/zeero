using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class UpgradeLibraryAuthoring : MonoBehaviour
{
    public UpgradeLibrary UpgradeLibrary;
}

class UpgradeLibraryAuthoringBaker : Baker<UpgradeLibraryAuthoring>
{
    public override void Bake(UpgradeLibraryAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new UpgradeLibraryBlob()
        {
            AllUpgrades = BlobUtils.CreateBlobArrayRefFromList(authoring.UpgradeLibrary.Upgrades, (so, index) =>
            {
                return new UpgradeDefinition()
                {
                    ID = index,
                    Name = new FixedString64Bytes(so.Name),
                    Description = new FixedString64Bytes(so.Description),
                    Rarity = so.Rarity,
                    Duration = so.Duration,
                    StatMods = so.Modifiers
                };
            })
        });
    }
}

public struct UpgradeLibraryBlob : IComponentData
{
    public BlobAssetReference<BlobArray<UpgradeDefinition>> AllUpgrades;
}

[System.Serializable]
public struct UpgradeDefinition
{
    public int ID;
    public FixedString64Bytes Name;
    public FixedString64Bytes Description;
    public Rarity Rarity;
    public PlayerStatsModifiers StatMods;
    public float Duration;
}
