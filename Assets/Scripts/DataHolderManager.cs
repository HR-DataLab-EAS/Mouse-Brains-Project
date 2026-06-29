using UnityEngine;

/// <summary>
/// DataHolderManager — Tracks the single loaded model under Data Holder.
/// Both GLBLoader and PrefabLoader call ClearLoaded() before spawning,
/// so loading any type always removes whatever was there before.
///
/// SETUP: Attach to your "Data Holder" GameObject.
/// </summary>
public class DataHolderManager : MonoBehaviour
{
    private GameObject _loaded;

    /// <summary>
    /// Destroys the currently loaded model (if any) and clears the reference.
    /// Call this before instantiating a new model.
    /// </summary>
    public void ClearLoaded()
    {
        if (_loaded != null)
        {
            Destroy(_loaded);
            _loaded = null;
            Debug.Log("[DataHolderManager] Cleared previous model.");
        }
    }

    /// <summary>
    /// Register the newly spawned model so it can be cleared next time.
    /// </summary>
    public void RegisterLoaded(GameObject model)
    {
        _loaded = model;
    }
}
