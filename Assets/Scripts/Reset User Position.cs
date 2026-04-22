using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace Resets.User
{
    public class ResetUserPosition : MonoBehaviour
    {
        [Header("References")] // References to the VR rig and camera
        public XROrigin xrOrigin;
        public Transform cameraTransform;

        [Header("Target Position/Rotation")] // References for the target position and rotation to reset to
        public Vector3 targetHeadPosition = new Vector3(0f, 0f, 0f);
        public float targetYRotation = 0f;
        public bool resetRotation = true;

        public void Resetuser() // Method to reset the user's position and rotation
        {
            if (resetRotation)
            {
                AlignRotation();
            }

            AlignPosition();
        }

        public void AlignRotation() // Method to align the user's rotation with the target rotation
        {
            Transform originTransform = xrOrigin.transform; // Save a reference to the XR Origin's transform

            float currentCameraYaw = cameraTransform.eulerAngles.y; // Get the current yaw rotation of the camera (the horizontal rotation around the Y axis)
            float yawDelta = targetYRotation - currentCameraYaw; // Calculate the difference between the target yaw and the current camera yaw

            // Rotate the XR Origin around the camera's position by the calculated yaw delta
            originTransform.RotateAround
            (
                new Vector3(cameraTransform.position.x, originTransform.position.y, cameraTransform.position.z), // Rotate around the camera's x and z, but keep the y position of the XR Origin
                Vector3.up, // Rotate around the Y axis
                yawDelta // Rotate by the calculated yaw delta to align the camera's yaw with the target yaw
            );
        }

        public void AlignPosition() // Method to align the user's position with the target position
        {
            Transform originTransform = xrOrigin.transform; // Save a reference to the XR Origin's transform
            Vector3 currentHeadPos = cameraTransform.position; // Get the current position of the camera
            Vector3 delta = targetHeadPosition - currentHeadPos; // Calculate the difference between the target head position and the current camera position

            delta.y = targetHeadPosition.y - currentHeadPos.y; // Only consider the vertical difference for the Y axis
            originTransform.position += delta; // Move the XR Origin by the calculated delta to align the camera's position with the target head position
        }
    }
}