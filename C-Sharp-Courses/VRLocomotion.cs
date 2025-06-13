using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This class demonstrates different types of VR locomotion methods
/// </summary>
public class VRLocomotion : MonoBehaviour
{
    [Header("Locomotion Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private float snapTurnAngle = 45f;
    [SerializeField] private float teleportDistance = 10f;
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;

    private Vector2 leftThumbstick;
    private Vector2 rightThumbstick;
    private bool isTeleporting;

    private void Start()
    {
        // Get required components if not assigned
        if (xrOrigin == null)
            xrOrigin = GetComponent<XROrigin>();
        
        if (playerCamera == null && xrOrigin != null)
            playerCamera = xrOrigin.Camera.transform;
    }

    private void Update()
    {
        // Get input from controllers
        GetControllerInput();

        // Handle different locomotion methods
        HandleSmoothLocomotion();
        HandleSnapTurn();
        HandleTeleport();
    }

    private void GetControllerInput()
    {
        // Get thumbstick input from both controllers
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (leftDevice.isValid)
            leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftThumbstick);
        
        if (rightDevice.isValid)
            rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightThumbstick);
    }

    private void HandleSmoothLocomotion()
    {
        if (leftThumbstick.magnitude > 0.1f)
        {
            // Get the forward direction based on where the player is looking
            Vector3 forward = playerCamera.forward;
            forward.y = 0;
            forward.Normalize();

            // Get the right direction
            Vector3 right = playerCamera.right;
            right.y = 0;
            right.Normalize();

            // Calculate movement direction
            Vector3 movement = (forward * leftThumbstick.y + right * leftThumbstick.x) * moveSpeed * Time.deltaTime;
            
            // Apply movement
            xrOrigin.transform.position += movement;
        }
    }

    private void HandleSnapTurn()
    {
        if (Mathf.Abs(rightThumbstick.x) > 0.7f)
        {
            float turnAmount = Mathf.Sign(rightThumbstick.x) * snapTurnAngle;
            xrOrigin.transform.Rotate(Vector3.up, turnAmount);
        }
    }

    private void HandleTeleport()
    {
        // Check for teleport input (usually trigger press)
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed && !isTeleporting)
            {
                TryTeleport();
            }
        }
    }

    private void TryTeleport()
    {
        isTeleporting = true;

        // Cast a ray from the right controller
        Ray ray = new Ray(rightController.position, rightController.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, teleportDistance, groundLayer))
        {
            // Calculate the target position
            Vector3 targetPosition = hit.point;
            
            // Keep the same height as the player
            targetPosition.y = xrOrigin.transform.position.y;
            
            // Teleport the player
            xrOrigin.transform.position = targetPosition;
        }

        isTeleporting = false;
    }

    // Helper method to check if the player is grounded
    private bool IsGrounded()
    {
        return Physics.Raycast(playerCamera.position, Vector3.down, 1.1f, groundLayer);
    }

    // Helper method to get the current movement speed
    public float GetCurrentSpeed()
    {
        return leftThumbstick.magnitude * moveSpeed;
    }

    // Helper method to check if the player is moving
    public bool IsMoving()
    {
        return leftThumbstick.magnitude > 0.1f;
    }
} 