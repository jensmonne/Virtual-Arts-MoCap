using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    
    private Renderer renderer;
    private bool isActive;
    private PointGroup group;
    
    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void SetGroup(PointGroup pointGroup)
    {
        group = pointGroup;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isActive) return;
        isActive = true;
        renderer.material = green;
        group.OnPointActivated(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive) return;
        isActive = false;
        renderer.material = red;
        group.OnPointDeactivated(this);
    }
}