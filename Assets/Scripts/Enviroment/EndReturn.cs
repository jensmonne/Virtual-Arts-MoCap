using AscentProtocol.SceneManagement;
using UnityEngine;

public class EndReturn : MonoBehaviour
{
    private SceneLoader sceneLoader;
    
    private void Start()
    {
        sceneLoader = FindAnyObjectByType<SceneLoader>();
    }

    public void ReturnToMenu()
    {
        sceneLoader.Load("Museum", () => sceneLoader.Unload("Level2"));
        SceneObjectRegistry.Instance.Get("Player").transform.position = new Vector3(0f, 0f, -1.1f);
    }

    public void Unload()
    {
        sceneLoader.Unload("Level1");
    }
}
