using UnityEngine;
using AscentProtocol.SceneManagement;

public class Painting : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material material;
    private SceneLoader sceneLoader;

    public void Start()
    {
        sceneLoader = SceneObjectRegistry.Instance.Get("Manager").GetComponent<SceneLoader>();
    }
    
    public void UILoadWorld()
    {
        sceneLoader.Load("Level1");
    }
    
    public void OnHoverEnter()
    {
        meshRenderer.materials[1] = material;
    }

    public void OnHoverExit()
    {
        meshRenderer.materials[1] = null;
    }
}