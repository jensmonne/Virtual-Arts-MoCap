using UnityEngine;

public class StopTriggers : MonoBehaviour
{
    [SerializeField] private PointGroup pointGroup;

    private void Start()
    {
        if (pointGroup == null)
        {
            pointGroup = FindAnyObjectByType<PointGroup>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        pointGroup.EnablePoints();
    }
}
