// using UnityEngine;

// public class ResetPosition : MonoBehaviour
// {
//     [Header("XR Rig Setup")]
//     public Transform xrRig;
//     public Transform xrCamera;

//     [Header("Target")]
//     public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
//     public Vector3 targetRotation = new Vector3(0f, 0f, 0f);
//     public bool resetRotation = false;

//     public void Start()
//     {
//         Reset();
//     }

//     public void Reset()
//     {
//         if (resetRotation)
//         {
//             float currentYaw = xrCamera.eulerAngles.y;
//             float targetYaw = targetRotation.y;
//             float yawDelta = Mathf.DeltaAngle(currentYaw, targetYaw);
//             xrRig.Rotate(0f, yawDelta, 0f);
//         }

//         Vector3 headOffset = xrCamera.position - xrRig.position;
//         headOffset.y = 0f;

//         xrRig.position = targetPosition - headOffset;
//     }
// }

using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public GameObject objectToReset;

    // The position and rotation to reset to in Vector3 format (x, y, z)
    public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public Vector3 targetRotation = new Vector3(0f, 0f, 0f);

    public bool resetRotation = false;

    public void Reset()
    {
        objectToReset.transform.position = targetPosition;    
        if (resetRotation)
        {
            objectToReset.transform.localRotation = Quaternion.Euler(targetRotation); // Convert the target rotation from Euler angles to a Quaternion and apply it to the object's local rotation
        }
    }
}