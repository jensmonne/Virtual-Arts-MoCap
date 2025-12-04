using UnityEngine;

public class RokokoRootFix : MonoBehaviour
{
    [Header("Root Fix Settings")]
    [SerializeField] private bool lockToInitialPosition = true;
    [SerializeField] private bool lockToXROrigin = false;

    [Space(10)]
    [SerializeField] private Transform xrOrigin;   // vaše XR Origin of Camera Offset
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        // 1 — Lock rig to initial position (ignores Rokoko root motion)
        if (lockToInitialPosition && !lockToXROrigin)
        {
            transform.position = initialPosition;
        }

        // 2 — OR: Lock rig under the XR origin / headset
        if (lockToXROrigin && xrOrigin != null)
        {
            transform.position = xrOrigin.position + positionOffset;
            transform.rotation = xrOrigin.rotation * Quaternion.Euler(rotationOffset);
        }

        // ALWAYS: prevent Rokoko from rotating or drifting the whole rig
        transform.rotation = Quaternion.identity;
    }
}
