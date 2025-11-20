using UnityEngine;

public class FollowAxis : MonoBehaviour
{
    [Header("Target to read X/Z from")]
    [SerializeField] private Transform target;

    [Header("Choose Axis")]
    [SerializeField] private bool followX = true;

    [Header("Deadzone and Speed")]
    [SerializeField] private float deadZone = 0.1f;
    [SerializeField] private float followSpeed = 20f;

    private float baseValue;

    private void Start()
    {
        baseValue = followX ? target.position.x : target.position.z;
    }

    private void Update()
    {
        if (!target) return;

        float current = followX ? target.position.x : target.position.z;

        // Compute distance from base position
        float offset = current - baseValue;

        // If inside deadzone => do nothing
        if (Mathf.Abs(offset) < deadZone)
            return;

        // Clamp to edge of deadzone
        float clamped = baseValue + Mathf.Sign(offset) * deadZone;

        // Smoothly move object to the clamped boundary
        Vector3 pos = transform.position;

        if (followX)
            pos.x = Mathf.Lerp(pos.x, clamped, Time.deltaTime * followSpeed);
        else
            pos.z = Mathf.Lerp(pos.z, clamped, Time.deltaTime * followSpeed);

        transform.position = pos;

        // Once follower has caught up enough, shift the base
        if (Mathf.Abs((followX ? pos.x : pos.z) - clamped) < 0.01f)
        {
            // Move the deadzone center forward gradually
            baseValue = Mathf.Lerp(baseValue, current, Time.deltaTime * 2f);
        }
    }
}
