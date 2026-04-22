using System.IO;
using UnityEngine;
using GLTFast;
using MenuNamespace.Manager;

namespace MenuNamespace.GLB
{
    public class GLBLoader : MonoBehaviour
    {
        [Header("References")]
        public VRFileBrowser fileBrowser; // Reference to the VR file browser UI
        public Transform dataHolder; // The parent transform under which the loaded model will be placed

        [Header("Placement (local, inside Data Holder)")]
        public Vector3 localPosition = Vector3.zero; // Local position for the loaded model relative to the data holder
        public Vector3 localRotation = Vector3.zero; // Local rotation (in Euler angles) for the loaded model relative to the data holder
        public Vector3 localScale = Vector3.one; // Local scale for the loaded model relative to the data holder

        private GameObject loadedModel;

        public void OpenFileBrowser() // Method to open the file browser
        {
            // 
            fileBrowser.OnFileConfirmed -= OnFileConfirmed;
            fileBrowser.OnFileConfirmed += OnFileConfirmed;

            fileBrowser.SetExtensionFilter(new string[] { ".glb", ".gltf" }); // Set the file extension filter to show only GLB and GLTF files

            FindAnyObjectByType<MenuManager>().OpenFileBrowser(); // Open the file browser through the menu manager script
            fileBrowser.Open(); // Ensures the file browser opens
        }

        private void OnDestroy()
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed; // Unsubscribe from the file confirmed event when this object is destroyed
        }

        private void OnFileConfirmed(string path) // Method called when a file is confirmed in the file browser, with the file path as an argument
        {
            fileBrowser.OnFileConfirmed -= OnFileConfirmed; // Unsubscribe from the event to prevent multiple calls
            LoadGLBAsync(path); // Start loading the GLB file asynchronously
        }

        private async void LoadGLBAsync(string path) // Asynchronous method to load the GLB file
        {
            Destroy(loadedModel); // Destroy the previously loaded model if it exists
            loadedModel = null;
            
            string modelName = Path.GetFileNameWithoutExtension(path); // Get the file name without extension to use as the model's name
            GameObject container = new GameObject(modelName); // Create a new empty GameObject to serve as the container for the loaded model

            container.transform.SetParent(dataHolder, false); // Set the container's parent to the data holder without changing its local transform

            // Set the container's local position, rotation, and scale based on the specified values
            container.transform.localPosition = localPosition;
            container.transform.localEulerAngles = localRotation;
            container.transform.localScale = localScale;

            var gltf = new GltfImport(); // Create a new instance of the GLTF importer unity package
            string uri = "file:///" + path.Replace("\\", "/"); // Create the URI for the GLB file
            bool success = await gltf.Load(uri); // Load the GLB file asynchronously and wait for the result (success or failure)

            if (success) // If the GLB file was loaded successfully, instantiate the main scene of the GLB model as a child of the container GameObject
            {
                await gltf.InstantiateMainSceneAsync(container.transform);
                loadedModel = container;
            }
            else // If there was an error loading the GLB file, destroy the container GameObject
            {
                Destroy(container);
            }
        }
    }
}