using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    public GameObject menu;

    public void Close()
    {
        menu.SetActive(!menu.activeSelf);
    }
}