using UnityEngine;
using UnityEditor;


public class AddCollidersToGrabInteractable : MonoBehaviour
{
    [MenuItem("Tools/Fix XR Grab Colliders")]
    static void FixColliders()
    {
        foreach (var grab in FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>())
        {
            var colliders = grab.GetComponentsInChildren<Collider>();
            grab.colliders.Clear();
            foreach (var col in colliders)
                grab.colliders.Add(col);
            EditorUtility.SetDirty(grab);
        }
        Debug.Log("Done! Fixed all XR Grab Interactables in scene.");
    }
}