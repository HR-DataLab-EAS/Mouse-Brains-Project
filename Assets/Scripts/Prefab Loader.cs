using System.IO;
using UnityEngine;
using MenuNamespace.Manager;

namespace MenuNamespace.Prefab
{
    public class PrefabLoader : MonoBehaviour
    {
        [Header("References")]
        public VRFileBrowser fileBrowser;
        public Transform dataHolder;
        public GameObject myObject;

        [Header("Placement (local, inside Data Holder)")]
        public bool keepPrefabTransform = true;
        public Vector3 localPosition = Vector3.zero;
        public Vector3 localRotation = Vector3.zero;
        public Vector3 localScale = Vector3.one;

        public bool replaceExisting = true;

        private const string ResourcesSubfolder = "UserPrefabs";
        private DataHolderManager _dataHolderManager;

        private void Start()
        {
            if (dataHolder != null)
                _dataHolderManager = dataHolder.GetComponent<DataHolderManager>();

            if (_dataHolderManager == null)
                Debug.LogWarning("[PrefabLoader] No DataHolderManager found on Data Holder.");
        }

        private void OnDestroy()
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed;
        }

        public void OpenFileBrowser()
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

            if (prefab == null)
            {
                Debug.LogError($"[PrefabLoader] Could not load '{resourcePath}' from Resources.");
                return;
            }

            // Clear whatever is loaded — GLB or prefab
            _dataHolderManager?.ClearLoaded();

            GameObject instance = Instantiate(prefab);
            instance.name = prefabName;
            instance.transform.SetParent(dataHolder, false);

            if (!keepPrefabTransform)
            {
                instance.transform.localPosition = localPosition;
                instance.transform.localEulerAngles = localRotation;
                instance.transform.localScale = localScale;
            }

            _dataHolderManager?.RegisterLoaded(instance);
            Debug.Log("[PrefabLoader] ✅ Spawned: " + prefabName);

            myObject.SetActive(false);
        }

        private static string GetUserPrefabsPath()
        {
#if UNITY_EDITOR
                return Path.Combine(Application.dataPath, "Resources", ResourcesSubfolder);
#else
            return Path.Combine(Path.GetDirectoryName(Application.dataPath), ResourcesSubfolder);
#endif
        }
    }
}