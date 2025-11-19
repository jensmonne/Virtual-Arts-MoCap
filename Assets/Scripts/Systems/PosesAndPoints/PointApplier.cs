using System.Collections.Generic;
using UnityEngine;

public class PointApplier : MonoBehaviour
{
    [SerializeField] private Transform avatarRoot;
    [SerializeField] private GameObject pointPrefab;

    private Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
    private List<GameObject> spawnedPoints = new List<GameObject>();
    
    private void Awake()
    {
        CacheBones();
    }

    private void CacheBones()
    {
        boneMap.Clear();

        foreach (Transform t in avatarRoot.GetComponentsInChildren<Transform>(true))
        {
            boneMap[t.name] = t;
        }
    }

    public void ClearPoints()
    {
        bool removedAny = false;
        
        foreach (var p in spawnedPoints)
        {
            if (p != null)
                DestroyImmediate(p);
            removedAny = true;
        }
        spawnedPoints.Clear();

        if (removedAny && avatarRoot == null) return;
        
        Transform[] allChildren = avatarRoot.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            if (child.name.Contains("Point"))
            {
                DestroyImmediate(child.gameObject);
                removedAny = true;
            }
        }

        if (removedAny)
            Debug.Log("Fallback cleanup: Removed GameObjects containing 'Point'.");
        else
            Debug.Log("No points found to delete.");
    }

    public void SpawnPoints(PoseData pose)
    {
        if (avatarRoot == null)
        {
            Debug.LogError("Avatar root not assigned.");
            return;
        }

        if (pointPrefab == null)
        {
            Debug.LogError("Point prefab not assigned.");
            return;
        }

        if (boneMap.Count == 0)
            CacheBones();

        ClearPoints();

        foreach (var bone in pose.bones)
        {
            if (!boneMap.TryGetValue(bone.boneName, out Transform realBone))
            {
                Debug.LogWarning($"Bone not found: {bone.boneName}");
                continue;
            }

            // SPAWN POINT AT THE TRUE BONE POSITION
            Vector3 worldPos = realBone.position;

            Quaternion forcedRotation = Quaternion.Euler(0, 0, 0);
            GameObject point = Instantiate(pointPrefab, worldPos, forcedRotation);
            point.transform.SetParent(realBone, true);   // optional: keep it attached

            // Require trigger collider
            Collider col = point.GetComponent<Collider>();
            if (col == null)
                col = point.AddComponent<SphereCollider>();
            col.isTrigger = true;

            spawnedPoints.Add(point);
        }

        Debug.Log($"Spawned {spawnedPoints.Count} points for pose.");
    }
}