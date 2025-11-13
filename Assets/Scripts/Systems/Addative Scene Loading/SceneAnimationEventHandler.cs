using UnityEngine;

namespace AscentProtocol.SceneManagement
{
    public class SceneAnimationEventHandler : MonoBehaviour
    {
        [SerializeField] private SceneLoader sceneLoader;

        public void LoadScene(string sceneName)
        {
            if (!sceneLoader)
            {
                Debug.LogWarning("SceneLoader not assigned to SceneAnimationEventHandler!");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogWarning("sceneName is null or empty!");
                return;
            }
        
            sceneLoader.Load(sceneName);
        }
    
        public void UnloadScene(string sceneName)
        {
            if (!sceneLoader)
            {
                Debug.LogWarning("SceneLoader not assigned to SceneAnimationEventHandler!");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogWarning("sceneName is null or empty!");
                return;
            }
        
            sceneLoader.Unload(sceneName);
        }
    }
}