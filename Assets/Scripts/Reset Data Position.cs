using UnityEngine;

namespace Resets.Data
{
    public class ResetDataPosition : MonoBehaviour
    {
        [Header("Object whose children will be reset")]
        public GameObject objectToReset;

        [Header("Target Position/Rotation")]
        public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
        public Vector3 targetRotation = new Vector3(0f, 0f, 0f);

        public void ResetData()
        {
            if (objectToReset == null)
            {
                Debug.LogError("[ResetDataPosition] objectToReset is not assigned.");
                return;
            }

            int childCount = objectToReset.transform.childCount;
            Debug.Log($"[ResetDataPosition] Resetting {childCount} children on '{objectToReset.name}'.");

            if (childCount == 0)
            {
                Debug.LogWarning("[ResetDataPosition] No children found — nothing to reset.");
                return;
            }

            foreach (Transform child in objectToReset.transform)
            {
                child.localPosition = targetPosition;
                child.localRotation = Quaternion.Euler(targetRotation);
            }

            Debug.Log("[ResetDataPosition] Done.");
        }
    }
}