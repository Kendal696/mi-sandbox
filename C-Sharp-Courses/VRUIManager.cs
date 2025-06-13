using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;

/// <summary>
/// This class demonstrates how to implement UI interaction in VR
/// </summary>
public class VRUIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private float uiInteractionDistance = 2f;
    [SerializeField] private LayerMask uiLayer;
    [SerializeField] private float uiHoverDistance = 0.1f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color pressedColor = Color.green;

    private XRRayInteractor rayInteractor;
    private GameObject currentHoveredUI;
    private Button currentHoveredButton;
    private Image currentHoveredImage;

    private void Start()
    {
        // Get the ray interactor
        rayInteractor = GetComponent<XRRayInteractor>();
        
        if (rayInteractor == null)
        {
            Debug.LogError("XRRayInteractor component not found!");
            return;
        }

        // Configure the ray interactor for UI interaction
        ConfigureRayInteractor();
    }

    private void ConfigureRayInteractor()
    {
        // Set up the ray interactor for UI interaction
        rayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
        rayInteractor.maxRaycastDistance = uiInteractionDistance;
        rayInteractor.raycastMask = uiLayer;
    }

    private void Update()
    {
        if (rayInteractor == null) return;

        // Check for UI elements under the ray
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            // Check if we hit a UI element
            if (IsUIElement(hitObject))
            {
                HandleUIHover(hitObject);
            }
            else
            {
                ClearUIHover();
            }
        }
        else
        {
            ClearUIHover();
        }
    }

    private bool IsUIElement(GameObject obj)
    {
        return obj.GetComponent<Button>() != null || 
               obj.GetComponent<Image>() != null || 
               obj.GetComponent<Text>() != null;
    }

    private void HandleUIHover(GameObject uiElement)
    {
        // If we're hovering over a new UI element
        if (currentHoveredUI != uiElement)
        {
            ClearUIHover();
            currentHoveredUI = uiElement;

            // Get UI components
            currentHoveredButton = uiElement.GetComponent<Button>();
            currentHoveredImage = uiElement.GetComponent<Image>();

            // Apply hover effect
            if (currentHoveredImage != null)
            {
                currentHoveredImage.color = hoverColor;
            }
        }

        // Check for button press
        if (currentHoveredButton != null)
        {
            if (rayInteractor.selectAction.action.triggered)
            {
                currentHoveredButton.onClick.Invoke();
                if (currentHoveredImage != null)
                {
                    currentHoveredImage.color = pressedColor;
                }
            }
        }
    }

    private void ClearUIHover()
    {
        if (currentHoveredImage != null)
        {
            currentHoveredImage.color = normalColor;
        }

        currentHoveredUI = null;
        currentHoveredButton = null;
        currentHoveredImage = null;
    }

    // Helper method to check if a UI element is within interaction distance
    public bool IsUIInRange(GameObject uiElement)
    {
        if (uiElement == null) return false;

        float distance = Vector3.Distance(transform.position, uiElement.transform.position);
        return distance <= uiInteractionDistance;
    }

    // Helper method to get the current hovered UI element
    public GameObject GetCurrentHoveredUI()
    {
        return currentHoveredUI;
    }
} 