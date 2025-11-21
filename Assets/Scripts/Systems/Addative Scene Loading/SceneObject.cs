using UnityEngine;

namespace AscentProtocol.SceneManagement
{
    /// <summary>
    /// Base component for any object that should be discoverable across scenes.
    /// Automatically registers and unregisters itself with the SceneObjectRegistry.
    /// </summary>
    public class SceneObject : MonoBehaviour
    {
        [Tooltip("Unique identifier for this object, used for cross-scene access")]
        [SerializeField] private string objectID = "";
    
        public string ObjectID => objectID;

        protected virtual void Awake()
        {
            if (string.IsNullOrWhiteSpace(objectID))
            {
                objectID = gameObject.name;
                Debug.LogWarning($"[SceneObject] Missing ObjectID on '{gameObject.name}'. Assigned default ID: '{objectID}'", this);
            }
        
            SceneObjectRegistry.Register(this);
        }
    }
}