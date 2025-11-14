using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject oldpointPrefab;
    [SerializeField] private GameObject pointGroupPrefab;
    private bool t;

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
        if (!t)
        {
            NextPose();
        }
        else
        {
            pointsText.text = "You did it!!! 😁👌";
        }
    }

    private void NextPose()
    {
        oldpointPrefab.SetActive(false);
        pointGroupPrefab.SetActive(true);
        t = true;
    }
}