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

    private void ClearPoints()
    {
        foreach (var p in spawnedPoints)
        {
            if (p != null)
                Destroy(p);
        }
        spawnedPoints.Clear();
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