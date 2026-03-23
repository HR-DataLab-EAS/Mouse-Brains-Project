using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject importMenu;
    public GameObject mathMenu;
    public GameObject saveMenu;
    public GameObject settingsMenu;

    public InputActionReference toggleMenuAction;

    public void Update()
    {
        if (toggleMenuAction.action.WasPressedThisFrame())
        {
            importMenu.SetActive(false);
            mathMenu.SetActive(false);
            saveMenu.SetActive(false);
            settingsMenu.SetActive(false);

            mainMenu.SetActive(!mainMenu.activeSelf);
        }
    }

    public void OpenImportMenu()
    {
        mainMenu.SetActive(false);
        importMenu.SetActive(true);
    }

    public void OpenMathMenu()
    {
        mainMenu.SetActive(false);
        mathMenu.SetActive(true);
    }

    public void OpenSaveMenu()
    {
        mainMenu.SetActive(false);
        saveMenu.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}