using System.Collections.Generic;
using UnityEngine;

public class PointGroup : MonoBehaviour
{
    private List<Point> points = new List<Point>();
    private int activeCount = 0;
    private PointManager manager;

    private void Start()
    {
        manager = PointManager.Instance;
        foreach (var point in GetComponentsInChildren<Point>())
        {
            points.Add(point);
            point.SetGroup(this);
        }
    }
    
    public void OnPointActivated(Point point)
    {
        activeCount++;
        CheckCompletion();
    }
    
    public void OnPointDeactivated(Point point)
    {
        activeCount--;
    }
    
    private void CheckCompletion()
    {
        if (activeCount != points.Count)
        {
            Debug.Log($"{activeCount} / {points.Count} Activated");
            return;
        }
        
        Debug.Log($"Group {name} completed!");
        manager.OnGroupCompleted(this);
    }
}