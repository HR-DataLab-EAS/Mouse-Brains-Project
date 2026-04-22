using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class VRFileBrowser : MonoBehaviour
{
    [Header("UI References")] // References for the UI elements of the file browser
    public Transform contentParent;
    public GameObject rowPrefab;
    public TMP_Text currentPathText;
    public TMP_Text selectedFileText;
    public Button upButton;
    public Button confirmButton;
    public Button closeButton;
    public TMP_Text statusText;

    [Header("File Filter (overridden at runtime by loaders)")]
    public string[] allowedExtensions = { ".glb", ".gltf", ".prefab" }; // Allowed extensions the user can select

    [Header("Colors")] // Colors for UI elements
    public Color colorDrive = new Color(0.80f, 0.70f, 1.00f);
    public Color colorFolder = new Color(1.00f, 0.85f, 0.30f);
    public Color colorFile = new Color(0.75f, 0.95f, 0.75f);
    public Color colorSelected = new Color(0.30f, 0.80f, 1.00f);

    public bool useCurrentUserDownloadsAsStart = true; // Bool to start in the users download folder

    public event Action<string> OnFileConfirmed; // Event for when a file is confirmed, giving the file path as string

    private string currentPath  = ""; // The current path the user is browsing
    private string selectedPath = ""; // The currently selected file path
    private readonly List<GameObject> rows = new List<GameObject>(); // List to keep track of the spawned UI rows for drives, folders, and files

    private void Awake() // 
    {
        upButton.onClick.AddListener(NavigateUp); // add a listener to a button to use it
        closeButton.onClick.AddListener(Close);
        confirmButton.onClick.AddListener(ConfirmSelection);

        if (confirmButton != null) confirmButton.interactable = false; // Disable the confirm button until a file is selected
    }

    public void SetExtensionFilter(string[] extensions) // Method to only show allowed extensions
    {
        allowedExtensions = extensions;
    }

    public void Open() // Method to open the file browser
    {
        selectedPath = "";
        gameObject.SetActive(true);

        if (useCurrentUserDownloadsAsStart)
        {
            string downloads = Path.Combine
                (
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"
                );

            if (Directory.Exists(downloads)) 
            { 
                NavigateTo(downloads); return; 
            }
        }

        ShowDrives();
    }

    public void Open(string startPath) // Method to open the file browser with a specified start path
    {
        selectedPath = "";
        gameObject.SetActive(true);

        if (Directory.Exists(startPath))
        {
            NavigateTo(startPath);
        }
        else
        {
            ShowDrives();
        }
    }

    public void Close() // Method to close the file browser
    {
        gameObject.SetActive(false);
        selectedPath = "";
    }

    private void ShowDrives() // Method to show the available drives on the system as the starting point for browsing
    {
        currentPath = "";
        SetPathLabel("My Computer");
        ClearRows();

        foreach (string drive in Directory.GetLogicalDrives())
        {
            string d = drive;
            SpawnRow("💾  " + d, colorDrive, () => NavigateTo(d));
        }

        if (upButton != null) 
        {
            upButton.interactable = false;
        }
        if (confirmButton != null) 
        {
            confirmButton.interactable = false;
        }
        if (selectedFileText != null) 
        {
            selectedFileText.text = "No file selected";
        }
        SetStatus("Select a drive to browse");
    }

    private void NavigateTo(string path) // Method to navigate to a specified path and display its contents
    {
        if (!Directory.Exists(path)) 
        { 
            SetStatus("Cannot open: " + path); return; 
        }

        currentPath  = path;
        selectedPath = "";
        SetPathLabel(currentPath);
        ClearRows();

        if (upButton != null) 
        {
            upButton.interactable = true;
        }
        if (confirmButton != null) 
        {
            confirmButton.interactable = false;
        }
        if (selectedFileText != null) 
        {
            selectedFileText.text = "No file selected";
        }

        string[] dirs = SafeGetDirs(currentPath);
        string[] files = SafeGetFiles(currentPath, allowedExtensions);

        foreach (string dir in dirs)
        {
            string d = dir;
            SpawnRow("📁  " + Path.GetFileName(d), colorFolder, () => NavigateTo(d));
        }

        foreach (string file in files)
        {
            string f = file;
            SpawnRow("📄  " + Path.GetFileName(f), colorFile, () => SelectFile(f));
        }

        if (dirs.Length == 0 && files.Length == 0)
        {
            SetStatus("Empty folder");
        }
        else
        {
            SetStatus($"{dirs.Length} folder(s)   •   {files.Length} file(s)");
        }
    }

    private void NavigateUp() // Method to navigate up to the parent directory
    {
        if (string.IsNullOrEmpty(currentPath)) return;
        DirectoryInfo parent = Directory.GetParent(currentPath);
        NavigateTo(parent.FullName);
    }

    private void SelectFile(string path) // Method to select a file and update the UI accordingly
    {
        selectedPath = path;

        foreach (GameObject row in rows)
        {
            TMP_Text lbl = row.GetComponentInChildren<TMP_Text>();
            if (!lbl.color.Equals(colorFolder) && !lbl.color.Equals(colorDrive))
            {
                lbl.color = lbl.text.Contains(Path.GetFileName(path)) ? colorSelected : colorFile;
            }
        }

        if (selectedFileText != null) 
        {
            selectedFileText.text = Path.GetFileName(path);
        }
        if (confirmButton != null) 
        {
            confirmButton.interactable = true;
        }
        SetStatus("Selected: " + Path.GetFileName(path) + "   —   Press Confirm to load");
    }

    private void ConfirmSelection() // Method to confirm the selected file and invoke the OnFileConfirmed event with the selected file path
    {
        if (string.IsNullOrEmpty(selectedPath)) return;
        string path = selectedPath;
        Close();
        OnFileConfirmed?.Invoke(path);
    }

    private void SpawnRow(string label, Color color, UnityEngine.Events.UnityAction onClick) // Method to spawn a UI for a specified type
    {
        GameObject go = Instantiate(rowPrefab, contentParent);
        go.SetActive(true);

        TMP_Text tmp = go.GetComponentInChildren<TMP_Text>();
        Button   btn = go.GetComponent<Button>();
        if (tmp != null) 
        {
            tmp.text = label; tmp.color = color;
        }
        if (btn != null)   
        {
            btn.onClick.AddListener(onClick);
        }

        rows.Add(go);
    }

    private void ClearRows() // Method to clear the spawned UI rows
    {
        foreach (var go in rows) Destroy(go);
        rows.Clear();
    }

    private void SetPathLabel(string path) // Method to set the current path label, shortening it if it's too long to fit
    {
        currentPathText.text = path.Length > 55 ? "…" + path[^55..] : path;
    }

    private void SetStatus(string msg) // Method to set the status text in the UI
    {
        if (statusText != null) 
        {
            statusText.text = msg;
        }
    }

    private static string[] SafeGetDirs(string path) // Method to safely get the directories in a specified path
    {
        try 
        { 
            return Directory.GetDirectories(path); 
        }
        catch 
        { 
            return Array.Empty<string>(); 
        }
    }

    private static string[] SafeGetFiles(string path, string[] exts) // Method to safely get the files in a specified path that match the allowed extensions
    {
        var list = new List<string>();
        foreach (string ext in exts)
        {
            try
            {
                string pattern = "*" + (ext.StartsWith(".") ? ext : "." + ext);
                list.AddRange(Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly));
            }
            catch { }
        }
        return list.ToArray();
    }
}