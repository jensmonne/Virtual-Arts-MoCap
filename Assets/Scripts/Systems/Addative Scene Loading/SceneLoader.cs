using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AscentProtocol.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        private readonly HashSet<string> _loadedScenes = new();

        private void Awake()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                _loadedScenes.Add(scene.name);
            }
        }

        public void Load(string sceneName, Action onComplete = null)
        {
            if (_loadedScenes.Contains(sceneName))
            {
                Debug.Log($"Scene '{sceneName}' is already loaded.");
                onComplete?.Invoke();
                return;
            }
        
            StartCoroutine(LoadAsync(sceneName, onComplete));
        }
    
        private IEnumerator LoadAsync(string sceneName, Action onComplete)
        {
            Debug.Log($"Loading scene: {sceneName}");
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (operation == null)
            {
                Debug.LogWarning($"Loading scene '{sceneName}' failed.");
                yield break;
            }
            yield return new WaitUntil(() => operation.isDone);

            _loadedScenes.Add(sceneName);
            Debug.Log($"Scene '{sceneName}' loaded successfully.");

            onComplete?.Invoke();
        }
    
        public void Unload(string sceneName, Action onComplete = null)
        {
            if (!_loadedScenes.Contains(sceneName))
            {
                Debug.Log($"Scene '{sceneName}' is not loaded.");
                return;
            }
        
            StartCoroutine(UnloadAsync(sceneName, onComplete));
        }
    
        private IEnumerator UnloadAsync(string sceneName, Action onComplete)
        {
            Debug.Log($"Unloading scene: {sceneName}");
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

            yield return new WaitUntil(() => operation.isDone);

            _loadedScenes.Remove(sceneName);
            Debug.Log($"Scene '{sceneName}' unloaded successfully.");

            onComplete?.Invoke();
        }

        public bool IsLoaded(string sceneName)
        {
            return _loadedScenes.Contains(sceneName) || SceneManager.GetSceneByName(sceneName).isLoaded;
        }
    }
}