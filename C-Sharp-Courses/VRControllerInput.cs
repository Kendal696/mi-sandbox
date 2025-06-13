using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class demonstrates basic VR controller input handling in Unity
/// </summary>
public class VRControllerInput : MonoBehaviour
{
    // Reference to the VR controller
    private InputDevice controller;

    private void Start()
    {
        // Initialize the controller
        InitializeController();
    }

    private void InitializeController()
    {
        // Get the right hand controller
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);

        if (inputDevices.Count > 0)
        {
            controller = inputDevices[0];
            Debug.Log($"Controller found: {controller.name}");
        }
        else
        {
            Debug.LogWarning("No VR controller found!");
        }
    }

    private void Update()
    {
        if (controller.isValid)
        {
            // Check for trigger press
            if (controller.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                if (triggerValue > 0.1f)
                {
                    Debug.Log($"Trigger pressed: {triggerValue}");
                }
            }

            // Check for grip press
            if (controller.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                if (gripValue > 0.1f)
                {
                    Debug.Log($"Grip pressed: {gripValue}");
                }
            }

            // Get controller position and rotation
            if (controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
            {
                transform.position = position;
            }

            if (controller.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
            {
                transform.rotation = rotation;
            }
        }
    }
} 