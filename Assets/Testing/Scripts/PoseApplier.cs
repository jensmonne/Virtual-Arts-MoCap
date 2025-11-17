using System.Collections.Generic;
using UnityEngine;

public class PoseApplier : MonoBehaviour
{
    [Tooltip("Root transform of the character skeleton")]
    [SerializeField] private Transform avatarRoot;

    private Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();

    private void Awake()
    {
        CacheBones();
    }

    private void CacheBones()
    {
        boneMap.Clear();

        foreach (Transform t in avatarRoot.GetComponentsInChildren<Transform>(true))
        {
            boneMap.TryAdd(t.name, t);
        }
    }

    public void ApplyPose(PoseData pose)
    {
        if (avatarRoot == null)
        {
            Debug.LogError("Avatar root not assigned!");
            return;
        }

        if (boneMap.Count == 0)
        {
            CacheBones();
        }

        foreach (var bone in pose.bones)
        {
            if (boneMap.TryGetValue(bone.boneName, out Transform target))
            {
                target.localPosition = bone.localPosition;
                target.localRotation = bone.localRotation;
            }
            else
            {
                Debug.LogWarning($"Bone '{bone.boneName}' not found on avatar '{avatarRoot.name}'");
            }
        }

        Debug.Log($"Applied pose: {pose.name}");
    }
}