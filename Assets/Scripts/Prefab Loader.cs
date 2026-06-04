using System.IO;
using UnityEngine;
using MenuNamespace.Manager;

namespace MenuNamespace.Prefab
{
    public class PrefabLoader : MonoBehaviour
    {
        [Header("References")] // references to the file browser and data holder for the loaded prefab
        public VRFileBrowser fileBrowser;
        public Transform dataHolder;
        public GameObject myObject;

        [Header("Placement (local, inside Data Holder)")] // options for how to position, rotate, and scale the loaded prefab relative to the data holder
        public bool keepPrefabTransform = true;
        public Vector3 localPosition = Vector3.zero;
        public Vector3 localRotation = Vector3.zero;
        public Vector3 localScale = Vector3.one;

        [Header("Behaviour")]
        public bool replaceExisting = true; // If true, loading a new prefab will replace the previously loaded one. If false, multiple prefabs can be loaded without replacing each other.

        private const string ResourcesSubfolder = "UserPrefabs"; // The name of the subfolder within the Resources folder where user prefabs are stored.
        private GameObject loadedInstance; 

        private void OnDestroy() // Unsubscribe from the file confirmed event when this object is destroyed
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed;
        }

        public void OpenFileBrowser() // Refer to GLB Loader (same method)
        {
            string prefabPath = GetUserPrefabsPath();

            fileBrowser.OnFileConfirmed -= OnFileConfirmed;
            fileBrowser.OnFileConfirmed += OnFileConfirmed;

            fileBrowser.SetExtensionFilter(new string[] { ".prefab" });

            FindAnyObjectByType<MenuManager>().OpenFileBrowser();
            fileBrowser.Open(prefabPath);
        }

        private void OnFileConfirmed(string path)
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed;

            string prefabName = Path.GetFileNameWithoutExtension(path);
            string resourcePath = ResourcesSubfolder + "/" + prefabName;
            GameObject prefab = Resources.Load<GameObject>(resourcePath);

            if (replaceExisting && loadedInstance != null) // Remove the old prefab under the specified conditions
            {
                Destroy(loadedInstance);
                loadedInstance = null;
            }

            loadedInstance = Instantiate(prefab); // Instantiate the loaded prefab and keep a reference to the instance
            loadedInstance.name = prefabName; // Set the name of the loaded instance to the prefabs name

            loadedInstance.transform.SetParent(dataHolder, false); // Set the parent of the loaded instance to the data holder without changing its local transform

            if (keepPrefabTransform != true)
            {
                // Use the manual transform instead of the prefab's original transform
                loadedInstance.transform.localPosition = localPosition;
                loadedInstance.transform.localEulerAngles = localRotation;
                loadedInstance.transform.localScale = localScale;
            }
            myObject.SetActive(false);
        }

        private static string GetUserPrefabsPath()
        {
            // the # makes the code compile before anything else when loading the scene
            #if UNITY_EDITOR // the code checks if it's running in the Unity Editor or in a aplication. And then it goes to the required path for the prefabs
                return Path.Combine(Application.dataPath, "Resources", ResourcesSubfolder);
            #else
                return Path.Combine(Path.GetDirectoryName(Application.dataPath), ResourcesSubfolder);
            #endif
        }
    }
}