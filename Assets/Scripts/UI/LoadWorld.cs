using UnityEngine;
using AscentProtocol.SceneManagement;

public class LoadWorld : MonoBehaviour
{
    private SceneLoader sceneLoader;

    public void Start()
    {
        sceneLoader = SceneObjectRegistry.Instance.Get("Manager").GetComponent<SceneLoader>();
    }
    
    public void UILoadWorld()
    {
        sceneLoader.Load("Level1");
    }
}