using UnityEngine;

namespace MenuNamespace.Close
{
    public class CloseMenu : MonoBehaviour
    {
        [Header("Menu Reference")]
        public GameObject menu; // Reference to the menu GameObject that will be toggled on and off

        public void Close()
        {
            menu.SetActive(!menu.activeSelf); // Toggles the menu's active state
        }
    }
}