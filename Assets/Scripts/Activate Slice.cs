using UnityEngine;

public class ActivateSlice : MonoBehaviour
{
    void Start()
    {
        var slicer = GetComponent<Slice>();
        var sliceNormal = Vector3.up;
        var sliceOrigin = transform.position;

        slicer.ComputeSlice(sliceNormal, sliceOrigin);
        
        Debug.Log("Code ran!");
    }
}
