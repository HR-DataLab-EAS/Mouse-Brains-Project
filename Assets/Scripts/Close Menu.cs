using UnityEngine;

namespace MenuNamespace.Close
{
    public class CloseMenu : MonoBehaviour
    {
        [Header("Menu Reference")]
        public GameObject menu;

        public void Close()
        {
            menu.SetActive(!menu.activeSelf); // Toggles the menu's active state
        }
    }
}