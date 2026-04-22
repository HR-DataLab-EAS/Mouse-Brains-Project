using UnityEngine;

namespace Resets.Data
{
public class ResetDataPosition : MonoBehaviour
{
    [Header("Object to Reset")]
    public GameObject objectToReset; // The object whose position and rotation will be reset

    [Header("Target Position/Rotation")]
    public Vector3 targetPosition = new Vector3(0f, 0f, 0f); // The position to which the object will be reset
    public Vector3 targetRotation = new Vector3(0f, 0f, 0f); // The rotation (in Euler angles) to which the object will be reset

    public void Reset() // Method to reset the object's position and rotation
    {
        objectToReset.transform.position = targetPosition; // Reset the object's position to the target position
        objectToReset.transform.localRotation = Quaternion.Euler(targetRotation); // Reset the object's rotation to the target rotation (converted from Euler angles to a Quaternion)
    }
}
}