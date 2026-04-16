using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject importMenu;
    public GameObject analyseMenu;
    public GameObject exportMenu;
    public GameObject settingsMenu;

    public InputActionReference toggleMenuAction;

    public void Update()
    {
        if (toggleMenuAction.action.WasPressedThisFrame())
        {
            importMenu.SetActive(false);
            analyseMenu.SetActive(false);
            exportMenu.SetActive(false);
            settingsMenu.SetActive(false);

            mainMenu.SetActive(!mainMenu.activeSelf);
        }
    }

    public void OpenImportMenu()
    {
        mainMenu.SetActive(false);
        importMenu.SetActive(true);
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
}