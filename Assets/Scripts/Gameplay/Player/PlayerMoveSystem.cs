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
        
        foreach (var (movementData, inputs, transform) in 
                 SystemAPI.Query<RefRW<MovementData>, RefRO<PlayerInputs>, RefRW<LocalTransform>>())
        {
            var moveDirection = new float3(inputs.ValueRO.moveInput.x, inputs.ValueRO.moveInput.y, 0);
            moveDirection = math.normalizesafe(moveDirection);

            if (!moveDirection.Equals(float3.zero))
            {
                movementData.ValueRW.CurrentMoveDirection += moveDirection * movementData.ValueRO.Acceleration * Time.deltaTime;
            }
            else
            {
                movementData.ValueRW.CurrentMoveDirection *= (1f - movementData.ValueRO.Drag * SystemAPI.Time.DeltaTime);
            }

            float currentSpeed = math.length(movementData.ValueRO.CurrentMoveDirection);
            if (currentSpeed > movementData.ValueRO.MaxMoveSpeed)
            {
                movementData.ValueRW.CurrentMoveDirection = (movementData.ValueRO.CurrentMoveDirection / currentSpeed) *
                                                            movementData.ValueRO.MaxMoveSpeed;
            }

            transform.ValueRW = transform.ValueRW.Translate(movementData.ValueRW.CurrentMoveDirection * Time.deltaTime);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
