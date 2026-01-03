using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[UpdateInGroup(typeof(GameplaySystemGroup))]
partial struct PlayerMoveSystem : ISystem
{
    private EntityQuery _stateQuery;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameFSM>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameFSM = SystemAPI.GetSingletonEntity<GameFSM>();
        var gameState = SystemAPI.GetComponent<CurrentStateType>(gameFSM);

        if (gameState.Type != GameFSMStates.FIGHTING_STATE)
            return;
        
        foreach (var (movementState, modData, inputs, transform, baseStats, collider, entity) in 
                 SystemAPI.Query<RefRW<MovementState>, 
                 RefRO<PlayerStatsModifiers>, 
                 RefRO<PlayerInputs>, 
                 RefRW<LocalTransform>, 
                 RefRO<PlayerBaseStats>, 
                 RefRO<PhysicsCollider>>().WithEntityAccess())
        {
            var movementBaseStats = baseStats.ValueRO.MovementStats;
            var movementModStats = modData.ValueRO.MovementStatsModifiers;

            var modifiedAcceleration = ModifierUtils.CalculateModifiedStat(movementBaseStats.Acceleration, movementModStats.AccelerationModifier);
            var modifiedMaxSpeed = ModifierUtils.CalculateModifiedStat(movementBaseStats.MaxMoveSpeed, movementModStats.MaxMoveSpeedModifier);
            var modifiedDrag = ModifierUtils.CalculateModifiedStat(movementBaseStats.Drag, movementModStats.DragModifier);

            var moveDirection = new float3(inputs.ValueRO.moveInput.x, inputs.ValueRO.moveInput.y, 0);
            moveDirection = math.normalizesafe(moveDirection);

            if (!moveDirection.Equals(float3.zero))
            {
                movementState.ValueRW.CurrentMoveDirection += moveDirection * modifiedAcceleration * Time.deltaTime;
            }
            else
            {
                movementState.ValueRW.CurrentMoveDirection *= (1f - movementBaseStats.Drag * SystemAPI.Time.DeltaTime);
            }

            float currentSpeed = math.length(movementState.ValueRO.CurrentMoveDirection);
            if (currentSpeed > modifiedMaxSpeed)
            {
                movementState.ValueRW.CurrentMoveDirection = (movementState.ValueRO.CurrentMoveDirection / currentSpeed) *
                                                            modifiedMaxSpeed;
            }

            float3 desiredMovement = movementState.ValueRO.CurrentMoveDirection * SystemAPI.Time.DeltaTime;

            // COLLISION CHECK
            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            unsafe
            {
                ColliderCastInput input = new ColliderCastInput()
                {
                    Collider = collider.ValueRO.ColliderPtr,
                    Orientation = transform.ValueRO.Rotation,
                    Start = transform.ValueRO.Position,
                    End = transform.ValueRO.Position + desiredMovement
                };

                var collector = new IgnoreEntityCollector(entity);

                if (collisionWorld.CastCollider(input, ref collector))
                {
                    float3 normal = collector.ClosestHit.SurfaceNormal;
                    normal.z = 0;
                    normal = math.normalizesafe(normal);

                    float safeFraction = math.max(0, collector.ClosestHit.Fraction - 0.01f);
                    float3 moveToHit = desiredMovement * safeFraction;

                    float3 remainingMove = desiredMovement - moveToHit;
                    float3 slideMove = remainingMove - math.project(remainingMove, normal);

                    float3 recovery = float3.zero;
                    if (collector.ClosestHit.Fraction <= 0.001f)
                    {
                        recovery = normal * 0.01f;
                    }

                    desiredMovement = moveToHit + slideMove + recovery;

                    float3 currentVel = movementState.ValueRO.CurrentMoveDirection;
                    movementState.ValueRW.CurrentMoveDirection = currentVel - math.project(currentVel, normal);
                }
            }

            transform.ValueRW = transform.ValueRW.Translate(desiredMovement);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public struct IgnoreEntityCollector : ICollector<ColliderCastHit>
{
    public bool EarlyOutOnFirstHit => false;
    public float MaxFraction { get; }
    public int NumHits { get; private set; }

    private Entity _ignore;
    public ColliderCastHit ClosestHit;

    public IgnoreEntityCollector(Entity ignore)
    {
        _ignore = ignore;
        MaxFraction = 1f;
        NumHits = 0;
        ClosestHit = default;
    }

    public bool AddHit(ColliderCastHit hit)
    {
        // If the hit entity is the player, ignore it and keep looking!
        if (hit.Entity == _ignore) return false;

        // If this hit is closer than our previous closest hit, save it
        if (NumHits == 0 || hit.Fraction < ClosestHit.Fraction)
        {
            ClosestHit = hit;
            NumHits = 1;
            return true;
        }
        return false;
    }
}
