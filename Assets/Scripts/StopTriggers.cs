using AscentProtocol.SceneManagement;
using UnityEngine;

public class StopTriggers : MonoBehaviour
{
    [SerializeField] private PointGroup pointGroup;
    [SerializeField] private bool hasGroup = true;
    [SerializeField] private GameObject canvas;
    
    private bool hasActivated = false;

    private void Start()
    {
        if (pointGroup == null && hasGroup)
        {
            pointGroup = FindAnyObjectByType<PointGroup>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasActivated) return;
        
        hasActivated = true;
        
        SceneObjectRegistry.Instance.Get("Player").GetComponentInChildren<PlayerMover>().StopMovement();
        if (!hasGroup)
        {
            canvas.SetActive(true);
            return;
        }
        pointGroup.EnablePoints();
    }
}