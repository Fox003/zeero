using Unity.Entities;
using UnityEngine;

class FSMBaseAuthoring : MonoBehaviour
{
    
}

class FSMBaseAuthoringBaker : Baker<FSMBaseAuthoring>
{
    public override void Bake(FSMBaseAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        
        AddComponent<FSM>(entity);
        AddComponent<EnableStateRequest>(entity);
        AddComponent<DisableStateRequest>(entity);
        //AddComponent<FSMDefaultState>(entity);
    }
}
