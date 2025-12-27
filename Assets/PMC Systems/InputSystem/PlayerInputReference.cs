using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerInputReference : IComponentData
{
    public int PlayerID;
    public PlayerInputActions PlayerInput;
    public InputDevice Device;
}

public struct Player1Tag : IComponentData { }
public struct Player2Tag : IComponentData { }
