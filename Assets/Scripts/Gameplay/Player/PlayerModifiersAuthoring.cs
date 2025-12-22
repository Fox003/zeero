using Unity.Entities;
using UnityEngine;

class PlayerModifiersAuthoring : MonoBehaviour
{
    public float DefaultAdditiveModifierValue = 0f;
    public float DefaultMultiplyModifierValue = 1f;
}

class ModifiersAuthoringBaker : Baker<PlayerModifiersAuthoring>
{
    public override void Bake(PlayerModifiersAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new MovementModifier()
        {
            AccelerationModifier = new Modifier() 
            { 
                AdditiveMod = authoring.DefaultAdditiveModifierValue, 
                MultiplyMod = authoring.DefaultMultiplyModifierValue 
            },
            MoveSpeedModifier = new Modifier()
            {
                AdditiveMod = authoring.DefaultAdditiveModifierValue,
                MultiplyMod = authoring.DefaultMultiplyModifierValue
            },
        });

        AddComponent(entity, new WeaponModifier()
        {
            CooldownModifier = new Modifier()
            {
                AdditiveMod = authoring.DefaultAdditiveModifierValue,
                MultiplyMod = authoring.DefaultMultiplyModifierValue
            },
        });
    }
}
