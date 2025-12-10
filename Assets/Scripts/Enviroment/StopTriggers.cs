using AscentProtocol.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class StopTriggers : MonoBehaviour
{
    [SerializeField] private PointGroup pointGroup;
    [SerializeField] private bool StopPlayer = true;
    
    [SerializeField] private UnityEvent onTriggered;
    
    private bool hasActivated = false;

    private void Start()
    {
        if (pointGroup == null && StopPlayer)
        {
            pointGroup = FindAnyObjectByType<PointGroup>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasActivated) return;
        hasActivated = true;
        if (StopPlayer) 
            SceneObjectRegistry.Instance.Get("Player").GetComponentInChildren<PlayerMover>().StopMovement();
            pointGroup.EnablePoints();
        onTriggered?.Invoke();
    }
}