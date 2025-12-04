using UnityEngine;
using AscentProtocol.SceneManagement;

public class Painting : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material material;
    
    private Material[] materials;
    private SceneLoader sceneLoader;

    public void Start()
    {
        sceneLoader = SceneObjectRegistry.Instance.Get("Manager").GetComponent<SceneLoader>();
        materials = meshRenderer.materials;
    }
    
    public void UILoadWorld()
    {
        sceneLoader.Load("Level1");
    }
    
    public void OnHoverEnter()
    {
        var mats = meshRenderer.materials;
        mats[1] = material;
        meshRenderer.materials = mats;
    }

    public void OnHoverExit()
    {
        meshRenderer.materials = materials;
    }
}