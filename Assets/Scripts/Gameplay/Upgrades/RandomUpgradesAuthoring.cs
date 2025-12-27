using Unity.Entities;
using UnityEngine;

class RandomUpgradesAuthoring : MonoBehaviour
{
    
}

class RandomUpgradesAuthoringBaker : Baker<RandomUpgradesAuthoring>
{
    public override void Bake(RandomUpgradesAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent<CurrentUpgrades>(entity);
        AddComponent<RequestUpgradeRollTag>(entity);
        AddComponent(entity, new CurrentPlayerUpgrading()
        {
            PlayerID = 2
        });
    }
}

public struct CurrentUpgrades : IComponentData
{
    public int UpgradeID1;
    public int UpgradeID2;
    public int UpgradeID3;
}

public struct CurrentPlayerUpgrading : IComponentData 
{
    public int PlayerID;
}


public struct RequestUpgradeRollTag : IComponentData, IEnableableComponent { }
