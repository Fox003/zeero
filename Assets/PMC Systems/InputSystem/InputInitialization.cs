using System.Linq;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using static UnityEngine.EventSystems.EventTrigger;

partial struct InputInitialization : ISystem, ISystemStartStop
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    
    public void OnUpdate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    public void OnStartRunning(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        
        InputUser.listenForUnpairedDeviceActivity = 2;
        InputUser.onUnpairedDeviceUsed += (control, _) =>
        {
            if (!(control is ButtonControl)) 
                return;
            if (control.device.description.deviceClass == "Mouse") 
                return;

            var user = InputUser.PerformPairingWithDevice(control.device);
            var playerInput = new PlayerInputActions();
            playerInput.Enable();
            user.AssociateActionsWithUser(playerInput);
            
            // if keyboard also pair with mouse
            if (control.device.description.deviceClass == "Keyboard")
            {
                var mouse = InputSystem.devices.FirstOrDefault(d => d.description.deviceClass == "Mouse");
                if (mouse != null)
                    InputUser.PerformPairingWithDevice(mouse, user);
            }

            var inputEntity = entityManager.CreateEntity();
            entityManager.SetName(inputEntity, $"InputUser {InputUser.listenForUnpairedDeviceActivity}");
            entityManager.AddComponentObject(inputEntity, new PlayerInputReference()
            {
                PlayerID = InputUser.listenForUnpairedDeviceActivity,
                PlayerInput = playerInput,
                Device = control.device
            });

            switch (InputUser.listenForUnpairedDeviceActivity)
            {
                case 2:
                    entityManager.AddComponent<Player1Tag>(inputEntity);
                    break;
                case 1:
                    entityManager.AddComponent<Player2Tag>(inputEntity);
                    break;
            }

            entityManager.AddComponent<PlayerInputNeedsAssociation>(inputEntity);
            
            InputUser.listenForUnpairedDeviceActivity--;
        };
    }

    public void OnStopRunning(ref SystemState state)
    {
        
    }
}

public struct PlayerInputNeedsAssociation : IComponentData, IEnableableComponent{}
public class UIControllerPrefab : IComponentData
{
    public GameObject Value;
}
