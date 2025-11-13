using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace AscentProtocol.SceneManagement
{
    [RequireComponent(typeof(Collider))]
    public class SceneTrigger : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Scene References")]
        [Tooltip("Scenes to load when the player enters this trigger.")]
        [SerializeField] private SceneAsset[] scenesToLoad;
    
        [Tooltip("Scenes to unload when the player enters this trigger.")]
        [SerializeField] private SceneAsset[] scenesToUnload;
#endif
    
        [SerializeField, HideInInspector] private string[] loadSceneNames;
        [SerializeField, HideInInspector] private string[] unloadSceneNames;
    
        [Header("Trigger Settings")]
        [Tooltip("If true, this trigger will only activate once per play session.")]
        [SerializeField] private bool triggerOnce = true;
    
        [Header("Events")]
        [Tooltip("Called immediately when this trigger is activated.")]
        [SerializeField] private UnityEvent onTriggered;
    
        [Tooltip("Called only when scene load/unload operations have completed. Preferred for most use cases.")]
        [SerializeField] private UnityEvent onCompleted;
    
        private bool _hasTriggered = false;
        private int _pendingOperations = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || (triggerOnce && _hasTriggered)) return;

            _hasTriggered = true;
            onTriggered?.Invoke();
            SceneLoader loader = FindAnyObjectByType<SceneLoader>();
            if (!loader)
            {
                Debug.LogWarning("No SceneLoader found!");
                return;
            }
        
            HandleSceneLoading(loader);
        }
    
        private void HandleSceneLoading(SceneLoader loader)
        {
            _pendingOperations = 0;

            foreach (var scene in loadSceneNames)
            {
                if (!string.IsNullOrWhiteSpace(scene))
                {
                    _pendingOperations++;
                    loader.Load(scene, OnSceneOperationComplete);
                }
            }

            foreach (var scene in unloadSceneNames)
            {
                if (!string.IsNullOrWhiteSpace(scene))
                {
                    _pendingOperations++;
                    loader.Unload(scene, OnSceneOperationComplete);
                }
            }

            if (_pendingOperations == 0)
                onCompleted?.Invoke();
        }

        private void OnSceneOperationComplete()
        {
            _pendingOperations--;
            if (_pendingOperations <= 0)
                onCompleted?.Invoke();
        }
    
#if UNITY_EDITOR
        private void OnValidate()
        {
            SyncSceneNames();
        }

        private void SyncSceneNames()
        {
            bool changed = false;
        
            if (scenesToLoad != null)
            {
                List<string> names = new List<string>();
                foreach (var asset in scenesToLoad)
                {
                    if (asset != null)
                        names.Add(asset.name);
                }
            
                if (!AreArraysEqual(loadSceneNames, names))
                {
                    loadSceneNames = names.ToArray();
                    changed = true;
                }
            }
        
            if (scenesToUnload != null)
            {
                List<string> names = new List<string>();
                foreach (var asset in scenesToUnload)
                {
                    if (asset != null)
                        names.Add(asset.name);
                }
            
                if (!AreArraysEqual(unloadSceneNames, names))
                {
                    unloadSceneNames = names.ToArray();
                    changed = true;
                }
            }

            if (changed)
                EditorUtility.SetDirty(this);
        }
    
        private bool AreArraysEqual(string[] current, List<string> updated)
        {
            if (current == null || current.Length != updated.Count)
                return false;

            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] != updated[i])
                    return false;
            }

            return true;
        }
#endif
    }
}