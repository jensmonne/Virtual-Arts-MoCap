using UnityEngine;

public class Point : MonoBehaviour
{
    private bool isActive;
    private PointGroup group;

    public void SetGroup(PointGroup pointGroup)
    {
        group = pointGroup;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isActive) return;
        isActive = true;
        group.OnPointActivated();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive) return;
        isActive = false;
        group.OnPointDeactivated();
    }
}