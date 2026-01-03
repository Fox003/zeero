using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestEventSystem : InputSystemUIInputModule
{
    private InputDevice authorizedDevice;

    public override void Process()
    {
        InputControl activeControl = move.action.activeControl ?? submit.action.activeControl;

        if (activeControl != null)
        {
            InputDevice currentDevice = activeControl.device;

            if (authorizedDevice == null)
            {
                authorizedDevice = currentDevice;
                Debug.Log($"UI claimed by: {authorizedDevice.name}");
            }

            if (currentDevice != authorizedDevice)
            {
                Debug.Log($"Ignored input from unauthorized device: {currentDevice.name}");
                return;
            }
        }

        base.Process();
    }

    // Optional: Call this when a menu closes to let someone else take control
    public void ReleaseUI()
    {
        authorizedDevice = null;
    }

}