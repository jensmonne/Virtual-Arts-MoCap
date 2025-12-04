using UnityEngine;

public class RokokoAbsoluteRootLock : MonoBehaviour
{
    [Header("Root Lock Settings")]
    [SerializeField] private bool lockRotation = true;

    private Vector3 lockedPosition;
    private Quaternion lockedRotation;

    void Start()
    {
        // Capture the position & rotation of the rig at calibration time
        lockedPosition = transform.position;
        lockedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // FORCE the rig to stay at the exact calibration spot
        transform.position = lockedPosition;

        // Optional: also freeze world rotation so Rokoko cannot rotate the root
        if (lockRotation)
            transform.rotation = lockedRotation;
    }

    // Call this manually after calibration if needed
    public void RecalibrateRoot()
    {
        lockedPosition = transform.position;
        lockedRotation = transform.rotation;
    }
}
