using System.Collections.Generic;
using UnityEngine;

public class PointGroup : MonoBehaviour
{
    private List<Point> points = new List<Point>();
    private int activeCount = 0;
    private PointManager manager;
    private bool hasActivated = false;

    private void Start()
    {
        manager = PointManager.Instance;
        foreach (var point in GetComponentsInChildren<Point>())
        {
            points.Add(point);
            point.SetGroup(this);
        }
        
        DisablePoints();
    }
    
    public void OnPointActivated()
    {
        activeCount++;
        CheckCompletion();
    }
    
    public void OnPointDeactivated()
    {
        activeCount--;
    }
    
    private void CheckCompletion()
    {
        if (activeCount != points.Count || hasActivated)
        {
            //Debug.Log($"{activeCount} / {points.Count} Activated");
            return;
        }
        
        hasActivated = true;
        
        Debug.Log($"Group {name} completed!");
        manager.OnGroupCompleted(this);
    }

    private void DisablePoints()
    {
        foreach (var point in points)
        {
            point.gameObject.SetActive(false);
        }
    }

    public void EnablePoints()
    {
        foreach (var point in points)
        {
            point.gameObject.SetActive(true);
        }
    }
}