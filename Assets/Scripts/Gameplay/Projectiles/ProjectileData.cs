using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ProjectileData : IComponentData
{
    public float MovementSpeed;
    public float Lifetime;
    public float Damage;
}

public struct ProjectileState : IComponentData
{
    public float3 direction;
    public float CurrentLifetime;
}

public struct DamageRequest: IComponentData {}
public struct ProjectileDestroyRequest: IComponentData, IEnableableComponent {}

[Serializable]
public class ProjectileVFX : IComponentData
{
    public ParticleSystem DeathVFX;
    public ParticleSystem SpawnVFX;
}