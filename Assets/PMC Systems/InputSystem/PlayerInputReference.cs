using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerInputReference : IComponentData
{
    public PlayerInputActions PlayerInput;
    public InputDevice Device;
}
