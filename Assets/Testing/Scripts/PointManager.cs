using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    [SerializeField] private TMP_Text pointsText;

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
        Debug.Log($"✅ {group.name} completed!");
        pointsText.text = "You did it!!! 😁👌";
    }
}