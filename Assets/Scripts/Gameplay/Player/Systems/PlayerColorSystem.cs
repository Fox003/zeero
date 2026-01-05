using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerColorSystem : ISystem
{
    private BufferLookup<Child> childLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        childLookup = state.GetBufferLookup<Child>(true);
    }

    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;

        childLookup.Update(ref state);

        // Player 1
        foreach (var (player, entity) in SystemAPI.Query<Player>().WithEntityAccess())
        {
            if (player.PlayerID == 2) 
                ApplyManagedColor(entity, Color.cyan, ref childLookup, entityManager);
            else if (player.PlayerID == 1)
                ApplyManagedColor(entity, Color.magenta, ref childLookup, entityManager);
        }
    }

    private void ApplyManagedColor(Entity parent, Color color, ref BufferLookup<Child> childLookup, EntityManager em)
    {
        if (childLookup.HasBuffer(parent))
        {
            var children = childLookup[parent];
            foreach (var child in children)
            {
                // Check if this specific child entity has the managed SpriteRenderer component
                if (em.HasComponent<SpriteRenderer>(child.Value))
                {
                    // Fetch the actual Unity SpriteRenderer object
                    var renderer = em.GetComponentObject<SpriteRenderer>(child.Value);
                    renderer.color = color;
                }
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
