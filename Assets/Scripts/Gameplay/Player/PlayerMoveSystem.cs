using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using Math = Unity.Mathematics.Geometry.Math;

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
        
        foreach (var (movementState, modData, inputs, transform, baseStats) in 
                 SystemAPI.Query<RefRW<MovementState>, RefRO<PlayerStatsModifiers>, RefRO<PlayerInputs>, RefRW<LocalTransform>, RefRO<PlayerBaseStats>>())
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

            transform.ValueRW = transform.ValueRW.Translate(movementState.ValueRW.CurrentMoveDirection * Time.deltaTime);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
