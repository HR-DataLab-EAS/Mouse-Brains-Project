using UnityEngine;
using UnityEngine.InputSystem;

public class ResetObjectPositionOnInput : MonoBehaviour
{
    public InputActionReference resetPositionAction;
    public GameObject objectToReset;

    private void OnEnable()
    {
        resetPositionAction.action.performed += ResetPosition;
        resetPositionAction.action.performed += ResetRotation;
    }

    private void ResetPosition(InputAction.CallbackContext context)
    {
        objectToReset.transform.localPosition = Vector3.zero;
    }

    private void ResetRotation(InputAction.CallbackContext context)
    {
        objectToReset.transform.localRotation = Quaternion.identity;
    }

}