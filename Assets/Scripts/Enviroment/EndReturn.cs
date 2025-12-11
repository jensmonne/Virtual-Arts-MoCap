using AscentProtocol.SceneManagement;
using UnityEngine;

public class EndReturn : MonoBehaviour
{
    private SceneLoader sceneLoader;
    
    private void Start()
    {
        sceneLoader = FindAnyObjectByType<SceneLoader>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        sceneLoader.Load("Museum", () => sceneLoader.Unload("Level2"));
    }
}
