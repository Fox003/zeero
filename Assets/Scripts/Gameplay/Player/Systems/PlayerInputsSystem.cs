using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(ProjectileDeathSystem))]
partial struct PlayerInputsSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }
    
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (inputs, controllerReference) in  SystemAPI.Query<RefRW<PlayerInputs>, ControllerReference>().WithNone<PlayerNeedsInputAssociation>())
        {
            var playerInputRef = controllerReference.PlayerInputRef;
            
            inputs.ValueRW.isGamepad = playerInputRef.Device is Gamepad;
            inputs.ValueRW.moveInput = playerInputRef.PlayerInput.Game.Move.ReadValue<Vector2>();
            inputs.ValueRW.lookInput = playerInputRef.PlayerInput.Game.Look.ReadValue<Vector2>();
            inputs.ValueRW.PrimaryFire = playerInputRef.PlayerInput.Game.PrimaryFire.inProgress;

            Vector3 lm = new Vector3(
                playerInputRef.PlayerInput.Game.Look.ReadValue<Vector2>().x,
                playerInputRef.PlayerInput.Game.Look.ReadValue<Vector2>().y, 
                10f);
            
            Vector3 ml = Camera.main.ScreenToWorldPoint(lm);
            inputs.ValueRW.mouseLookInput = new float2(ml.x, ml.y);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
