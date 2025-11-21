using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AscentProtocol.SceneManagement
{
    /// <summary>
    /// Central registry for cross-scene object access. Persists across additive scenes.
    /// </summary>
    public class SceneObjectRegistry : MonoBehaviour
    {
        private static SceneObjectRegistry _instance;
    
        private readonly Dictionary<string, SceneObject> _registeredObjects = new();

        public static SceneObjectRegistry Instance
        {
            get
            {
                if (_instance != null) return _instance;
            
                var registryObject = new GameObject("[SceneObjectRegistry]");
                _instance = registryObject.AddComponent<SceneObjectRegistry>();
                DontDestroyOnLoad(registryObject);
                return _instance;
            }
        }
    
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        
            SceneManager.sceneUnloaded += HandleSceneUnloaded;
        }
    
        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= HandleSceneUnloaded;
            _registeredObjects.Clear();
        
            if (_instance == this)
                _instance = null;
        }
    
        /// <summary>
        /// Automatically removes all scene objects that belong to an unloaded scene.
        /// </summary>
        private void HandleSceneUnloaded(Scene unloadedScene)
        {
            var keysToRemove = new List<string>();

            foreach (var (key, sceneObject) in _registeredObjects)
            {
                if (sceneObject != null && sceneObject.gameObject.scene == unloadedScene)
                    keysToRemove.Add(key);
            }

            foreach (var key in keysToRemove)
                _registeredObjects.Remove(key);
        }
    
        /// <summary>
        /// Registers a SceneObject for global access.
        /// </summary>
        public static void Register(SceneObject sceneObject)
        {
            if (sceneObject == null || string.IsNullOrWhiteSpace(sceneObject.ObjectID)) return;

            if (Instance._registeredObjects.ContainsKey(sceneObject.ObjectID))
            {
                Debug.LogWarning($"Duplicate SceneObject ID detected: '{sceneObject.ObjectID}'. Overwriting existing reference.");
                Instance._registeredObjects[sceneObject.ObjectID] = sceneObject;
            }
            else Instance._registeredObjects.Add(sceneObject.ObjectID, sceneObject);
        }

        /// <summary>
        /// Unregisters a SceneObject when it is no longer active or needed.
        /// </summary>
        public static void Unregister(SceneObject sceneObject)
        {
            if (sceneObject == null) return;

            Instance._registeredObjects.Remove(sceneObject.ObjectID);
        }
    
        /// <summary>
        /// Retrieves a registered SceneObject by ID and casts it to the specified type.
        /// </summary>
        public T Get<T>(string objectID) where T : SceneObject
        {
            if (Instance._registeredObjects.TryGetValue(objectID, out SceneObject sceneObject))
                return sceneObject as T;
        
            return null;
        }
    
        /// <summary>
        /// Retrieves a registered SceneObject by ID.
        /// </summary>
        public SceneObject Get(string objectID)
        {
            Instance._registeredObjects.TryGetValue(objectID, out SceneObject sceneObject);
            return sceneObject;
        }
    }
}