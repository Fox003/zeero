using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public partial struct ProjectileHitJob : ICollisionEventsJob
{
    [ReadOnly] public ComponentLookup<ProjectileDamageData> ProjectileDamageLookup;
    public ComponentLookup<HealthState> HealthLookup;
    public EntityCommandBuffer.ParallelWriter ECB;

    public void Execute(CollisionEvent ev)
    {
        var a = ev.EntityA;
        var b = ev.EntityB;

        bool aIsProjectile = ProjectileDamageLookup.HasComponent(a);
        bool bIsProjectile = ProjectileDamageLookup.HasComponent(b);

        if (aIsProjectile && HealthLookup.HasComponent(b))
        {
            ApplyDamage(ev.BodyIndexA, a, b);
            ECB.DestroyEntity(ev.BodyIndexA, a);
        }
        else if (bIsProjectile && HealthLookup.HasComponent(a))
        {
            ApplyDamage(ev.BodyIndexB, b, a);
            ECB.DestroyEntity(ev.BodyIndexB, b);
        }
    }

    void ApplyDamage(int sortKey, Entity projectile, Entity target)
    {
        var health = HealthLookup[target];
        var dmg = ProjectileDamageLookup[projectile].Damage;

        health.CurrentHealth -= dmg;

        HealthLookup[target] = health;
    }
}
