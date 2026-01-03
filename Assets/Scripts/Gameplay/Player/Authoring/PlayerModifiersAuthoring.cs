using Unity.Entities;
using UnityEngine;

class PlayerModifiersAuthoring : MonoBehaviour
{
    
}

class ModifiersAuthoringBaker : Baker<PlayerModifiersAuthoring>
{
    public override void Bake(PlayerModifiersAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new PlayerStatsModifiers()
        {
            MovementStatsModifiers = new MovementModifiers()
            {
                MaxMoveSpeedModifier = new Modifier(),
                AccelerationModifier = new Modifier(),
                DragModifier = new Modifier(),
            },

            HealthStatsModifiers = new HealthModifiers()
            {
                MaxHealthModifier = new Modifier(),
                MaxShieldModifier = new Modifier(),
            }
        });

        AddComponent<ActiveModifier>(entity);
    }
}
