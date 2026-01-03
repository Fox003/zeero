using Unity.Collections;
using Unity.Entities;


partial struct PlayerInputAssociationSystem : ISystem
{
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInputNeedsAssociation>();
        state.RequireForUpdate<ControllerReference>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var orphanInputEntities = new NativeList<Entity>(Allocator.Temp);
        
        foreach (var (playerInput, playerInputRef) in SystemAPI.Query<RefRO<PlayerInputNeedsAssociation>>().WithEntityAccess())
        {
            orphanInputEntities.Add(playerInputRef);
        }

        foreach (var (player, controllerReference, entity) in SystemAPI.Query<RefRO<PlayerNeedsInputAssociation>, ControllerReference>().WithEntityAccess())
        {
            if (!orphanInputEntities.IsEmpty)
            {
                controllerReference.PlayerInputRef = SystemAPI.ManagedAPI.GetComponent<PlayerInputReference>(orphanInputEntities[0]);
                SystemAPI.SetComponentEnabled<PlayerInputNeedsAssociation>(orphanInputEntities[0], false);
                SystemAPI.SetComponentEnabled<PlayerNeedsInputAssociation>(entity, false);

                SystemAPI.SetComponent(entity, new Player() { PlayerID = controllerReference.PlayerInputRef.PlayerID });

                if (controllerReference.PlayerInputRef.PlayerID == 2)
                {
                    state.EntityManager.SetName(entity, "Cyan");
                }
                else if (controllerReference.PlayerInputRef.PlayerID == 1)
                {
                    state.EntityManager.SetName(entity, "Magenta");
                }

                orphanInputEntities.RemoveAt(0);
            }
        }
        
        orphanInputEntities.Dispose();
    }

    
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
