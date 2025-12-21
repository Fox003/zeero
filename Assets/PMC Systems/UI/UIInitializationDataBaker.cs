using Unity.Entities;
using UnityEngine;

class UIInitializationDataBaker : MonoBehaviour
{
    public InterfaceState InitInterface;
}

class UIInitializationDataBakerBaker : Baker<UIInitializationDataBaker>
{
    public override void Bake(UIInitializationDataBaker authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        
    }
}
