using UnityEngine;
using Unity.XR.CoreUtils;
using System.Collections;

/// <summary>
/// ResetUserPosition — Resets the VR player to a target position and forward-facing rotation.
/// Can be called from a UI button, and optionally runs automatically on scene load
/// after a short delay to let headset tracking settle.
/// </summary>
public class ResetUserPosition : MonoBehaviour
{
    [Header("XR References")]
    [Tooltip("Your scene's XR Origin GameObject.")]
    [SerializeField] private XROrigin xrOrigin;

    [Tooltip("The Main Camera (inside XR Origin > Camera Offset > Main Camera).")]
    [SerializeField] private Transform cameraTransform;

    [Header("Target")]
    [Tooltip("Where the player's HEAD should end up (world position).")]
    [SerializeField] private Vector3 targetHeadPosition = new Vector3(0f, 0f, 0f);

    [Tooltip("Which direction the player should face after reset. 0 = forward along +Z.")]
    [SerializeField] private float targetYRotation = 0f;

    [Tooltip("If false, only resets position but leaves the player facing whatever direction they were.")]
    [SerializeField] private bool resetRotation = true;

    [Header("On Load")]
    [Tooltip("Automatically reset the player when the scene loads.")]
    [SerializeField] private bool resetOnLoad = true;

    [Tooltip("Seconds to wait after load before resetting. " +
             "0.5–1s lets the headset tracking settle so the offset is read correctly.")]
    [SerializeField] private float resetDelay = 0.5f;

    // ─────────────────────────────────────────────────────────────────────────

    private void Start()
    {
        if (xrOrigin == null)
            xrOrigin = FindObjectOfType<XROrigin>();

        if (xrOrigin == null)
        {
            Debug.LogError("[ResetUserPosition] No XROrigin found. Please assign it.");
            return;
        }

        if (cameraTransform == null)
            cameraTransform = xrOrigin.Camera?.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("[ResetUserPosition] No camera found on XROrigin. Please assign cameraTransform.");
            return;
        }

        if (resetOnLoad)
            StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        // Wait for tracking to settle before reading the camera offset
        yield return new WaitForSeconds(resetDelay);
        ResetPlayer();
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Resets the player's head to targetHeadPosition facing targetYRotation.
    /// Call this from your UI button's OnClick.
    /// </summary>
    public void ResetPlayer()
    {
        if (xrOrigin == null || cameraTransform == null)
        {
            Debug.LogError("[ResetUserPosition] Missing references — cannot reset.");
            return;
        }

        if (resetRotation)
            AlignRotation();

        AlignPosition();

        Debug.Log("[ResetUserPosition] Player reset to " + targetHeadPosition);
    }

    // ─────────────────────────────────────────────────────────────────────────

    private void AlignRotation()
    {
        Transform originTransform = xrOrigin.transform;
        float currentCameraYaw = cameraTransform.eulerAngles.y;
        float yawDelta = targetYRotation - currentCameraYaw;

        originTransform.RotateAround(
            new Vector3(cameraTransform.position.x, originTransform.position.y, cameraTransform.position.z),
            Vector3.up,
            yawDelta
        );
    }

    private void AlignPosition()
    {
        Vector3 delta = targetHeadPosition - cameraTransform.position;
        xrOrigin.transform.position += delta;
    }
}
