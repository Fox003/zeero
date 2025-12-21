using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ProjectileMovementData : IComponentData
{
    public float3 TargetPosition;
    public MovementBehavior MovementBehavior;
    public float MovementSpeed;
    public float3 direction;
}

public enum MovementBehavior
{
    Straight,
    Cosine,
}

public struct ProjectileDeathData : IComponentData
{
    public bool slay;
}

public struct ProjectileSpawnData : IComponentData
{
    
}

public struct ProjectileLifeData : IComponentData
{
    public float Lifetime;
    public float CurrentLifetime;
}

public struct ProjectileDamageData : IComponentData
{
    public float Damage;
}

public struct DamageRequest: IComponentData {}
public struct ProjectileDestroyRequest: IComponentData, IEnableableComponent {}

[Serializable]
public class ProjectileVFX : IComponentData
{
    public ParticleSystem DeathVFX;
    public ParticleSystem SpawnVFX;
}