using System;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    
    public event Action<PointGroup> OnPoseCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void OnGroupCompleted(PointGroup group)  
    {
        Debug.Log($"Player has completed group {group.name}");
        OnPoseCompleted.Invoke(group);
    }
}