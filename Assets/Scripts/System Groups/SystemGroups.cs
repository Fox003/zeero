using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class GameplaySystemGroup : ComponentSystemGroup
{
    
}

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class FSMStateValidationGroup : ComponentSystemGroup
{

}
