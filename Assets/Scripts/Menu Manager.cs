using UnityEngine;
using UnityEngine.InputSystem;

namespace MenuNamespace.Manager
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Menu References")]
        public GameObject mainMenu; // object reference for the main menu
        public GameObject importMenu;
        public GameObject analyseMenu;
        public GameObject exportMenu;
        public GameObject settingsMenu;
        public GameObject fileBrowser;

        [Header("Input")]
        public InputActionReference toggleMenuAction; // reference to the input action for toggling the menu


        public void Update()
        {
            if (toggleMenuAction.action.WasPressedThisFrame()) // Check if the toggle menu action was triggered this frame
            {
                importMenu.SetActive(false); // Ensure all submenus and file browser are closed when toggling the main menu
                analyseMenu.SetActive(false);
                exportMenu.SetActive(false);
                settingsMenu.SetActive(false);
                fileBrowser.SetActive(false);


                mainMenu.SetActive(!mainMenu.activeSelf); // Toggle the main menu's active state
            }
        }


        public void OpenImportMenu() // Method to open submenu and close past menu
        {
            mainMenu.SetActive(false); // Close past menu
            importMenu.SetActive(true); // Open submenu
        }


        public void OpenAnalyseMenu()
        {
            mainMenu.SetActive(false);
            analyseMenu.SetActive(true);
        }


        public void OpenExportMenu()
        {
            mainMenu.SetActive(false);
            exportMenu.SetActive(true);
        }


        public void OpenSettingsMenu()
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }


        public void OpenFileBrowser()
        {
            importMenu.SetActive(false);
            fileBrowser.SetActive(true);
        }
    }
}