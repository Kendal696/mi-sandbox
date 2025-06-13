using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This class demonstrates how to implement teleportation in VR
/// </summary>
public class VRTeleportation : MonoBehaviour
{
    [Header("Teleportation Settings")]
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private LayerMask teleportLayerMask;
    [SerializeField] private float maxTeleportDistance = 20f;
    [SerializeField] private GameObject teleportMarker;
    [SerializeField] private Color validTeleportColor = Color.green;
    [SerializeField] private Color invalidTeleportColor = Color.red;

    private bool isTeleporting = false;
    private Vector3 targetPosition;
    private LineRenderer lineRenderer;

    private void Start()
    {
        // Get or add LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            SetupLineRenderer();
        }

        // Initialize teleport marker
        if (teleportMarker != null)
        {
            teleportMarker.SetActive(false);
        }
    }

    private void SetupLineRenderer()
    {
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = validTeleportColor;
        lineRenderer.endColor = validTeleportColor;
    }

    private void Update()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // Check if the hit point is within teleport range and on valid layer
            if (hit.distance <= maxTeleportDistance && 
                ((1 << hit.collider.gameObject.layer) & teleportLayerMask) != 0)
            {
                // Valid teleport location
                targetPosition = hit.point;
                UpdateTeleportMarker(true);
                UpdateLineRenderer(hit.point);
            }
            else
            {
                // Invalid teleport location
                UpdateTeleportMarker(false);
                UpdateLineRenderer(hit.point);
            }
        }
        else
        {
            // No valid hit point
            if (teleportMarker != null)
            {
                teleportMarker.SetActive(false);
            }
            lineRenderer.enabled = false;
        }
    }

    private void UpdateTeleportMarker(bool isValid)
    {
        if (teleportMarker != null)
        {
            teleportMarker.SetActive(true);
            teleportMarker.transform.position = targetPosition;
            // Update marker color based on validity
            var markerRenderer = teleportMarker.GetComponent<Renderer>();
            if (markerRenderer != null)
            {
                markerRenderer.material.color = isValid ? validTeleportColor : invalidTeleportColor;
            }
        }
    }

    private void UpdateLineRenderer(Vector3 endPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
    }

    public void Teleport()
    {
        if (isTeleporting) return;

        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.distance <= maxTeleportDistance && 
                ((1 << hit.collider.gameObject.layer) & teleportLayerMask) != 0)
            {
                isTeleporting = true;
                // Get the XR Origin (player)
                var xrOrigin = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.XROrigin>();
                if (xrOrigin != null)
                {
                    // Calculate the offset from the hit point
                    Vector3 heightOffset = new Vector3(0, xrOrigin.CameraInOriginSpaceHeight, 0);
                    xrOrigin.MoveCameraToWorldLocation(hit.point + heightOffset);
                }
                isTeleporting = false;
            }
        }
    }
} 