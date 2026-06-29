using System;
using System.IO;
using UnityEngine;
using GLTFast;
using MenuNamespace.Manager;

namespace MenuNamespace.GLB
{
    public class GLBLoader : MonoBehaviour
    {
        [Header("References")]
        public VRFileBrowser fileBrowser;
        public Transform dataHolder;

        [Header("Placement (local, inside Data Holder)")]
        public Vector3 localPosition = Vector3.zero;
        public Vector3 localRotation = Vector3.zero;
        public Vector3 localScale = Vector3.one;

        private DataHolderManager _dataHolderManager;

        private void Start()
        {
            if (dataHolder != null)
                _dataHolderManager = dataHolder.GetComponent<DataHolderManager>();

            if (_dataHolderManager == null)
                Debug.LogWarning("[GLBLoader] No DataHolderManager found on Data Holder.");
        }

        public void OpenFileBrowser()
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed;
            fileBrowser.OnFileConfirmed += OnFileConfirmed;

            fileBrowser.SetExtensionFilter(new string[] { ".glb", ".gltf" });

            FindAnyObjectByType<MenuManager>().OpenFileBrowser();

            // Open directly in Downloads/UserDownloads, fall back to Downloads if it doesn't exist
            string downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string userDownloads = Path.Combine(downloads, "UserDownloads");
            string startPath = Directory.Exists(userDownloads) ? userDownloads : downloads;

            fileBrowser.Open(startPath);
        }

        private void OnDestroy()
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed;
        }

        private void OnFileConfirmed(string path)
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed;
            LoadGLBAsync(path);
        }

        private async void LoadGLBAsync(string path)
        {
            // Clear whatever is loaded — GLB or prefab
            _dataHolderManager?.ClearLoaded();

            string modelName = Path.GetFileNameWithoutExtension(path);
            GameObject container = new GameObject(modelName);

            container.transform.SetParent(dataHolder, false);
            container.transform.localPosition = localPosition;
            container.transform.localEulerAngles = localRotation;
            container.transform.localScale = localScale;

            var gltf = new GltfImport();
            string uri = "file:///" + path.Replace("\\", "/");
            bool success = await gltf.Load(uri);

            if (success)
            {
                await gltf.InstantiateMainSceneAsync(container.transform);
                _dataHolderManager?.RegisterLoaded(container);
                Debug.Log("[GLBLoader] ✅ Loaded: " + modelName);
            }
            else
            {
                Debug.LogError("[GLBLoader] ❌ Failed to load: " + path);
                Destroy(container);
            }
        }
    }
}