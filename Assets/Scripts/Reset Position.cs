using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public GameObject objectToReset;

    public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public Vector3 targetRotation = new Vector3(0f, 0f, 0f);

    public bool resetRotation = false;

    public void Reset()
    {
        objectToReset.transform.position = targetPosition;    
        if (resetRotation)
        {
            objectToReset.transform.localRotation = Quaternion.Euler(targetRotation);
        }
    }
}