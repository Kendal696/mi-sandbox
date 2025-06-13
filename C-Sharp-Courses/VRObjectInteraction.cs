using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This class demonstrates how to implement object interaction in VR
/// </summary>
public class VRObjectInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private float grabDistance = 0.1f;
    [SerializeField] private LayerMask interactableLayer;

    private XRGrabInteractable currentGrabbedObject;
    private XRDirectInteractor directInteractor;
    private XRRayInteractor rayInteractor;

    private void Start()
    {
        // Get the interactors
        directInteractor = GetComponent<XRDirectInteractor>();
        rayInteractor = GetComponent<XRRayInteractor>();

        // Subscribe to interaction events
        if (directInteractor != null)
        {
            directInteractor.selectEntered.AddListener(OnGrab);
            directInteractor.selectExited.AddListener(OnRelease);
        }

        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.AddListener(OnGrab);
            rayInteractor.selectExited.AddListener(OnRelease);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (directInteractor != null)
        {
            directInteractor.selectEntered.RemoveListener(OnGrab);
            directInteractor.selectExited.RemoveListener(OnRelease);
        }

        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.RemoveListener(OnGrab);
            rayInteractor.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Get the grabbed object
        currentGrabbedObject = args.interactableObject as XRGrabInteractable;
        
        if (currentGrabbedObject != null)
        {
            // Configure the grabbed object
            ConfigureGrabbedObject(currentGrabbedObject);
            Debug.Log($"Grabbed object: {currentGrabbedObject.name}");
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (currentGrabbedObject != null)
        {
            // Apply throw force if the controller is moving
            ApplyThrowForce(currentGrabbedObject);
            currentGrabbedObject = null;
        }
    }

    private void ConfigureGrabbedObject(XRGrabInteractable grabbedObject)
    {
        // Configure physics properties
        if (grabbedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // You can add additional configuration here
        // For example, changing the object's material or enabling/disabling components
    }

    private void ApplyThrowForce(XRGrabInteractable releasedObject)
    {
        if (releasedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // Get the controller's velocity
            Vector3 controllerVelocity = directInteractor != null ? 
                directInteractor.velocityAction.action.ReadValue<Vector3>() : 
                rayInteractor.velocityAction.action.ReadValue<Vector3>();

            // Apply the throw force
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = controllerVelocity * throwForce;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // Helper method to check if an object is interactable
    public bool IsInteractable(GameObject obj)
    {
        return ((1 << obj.layer) & interactableLayer) != 0;
    }

    // Helper method to get the current grabbed object
    public XRGrabInteractable GetCurrentGrabbedObject()
    {
        return currentGrabbedObject;
    }
} 