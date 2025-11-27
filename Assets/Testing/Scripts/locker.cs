using UnityEngine;

public class LockX : MonoBehaviour
{
    [Header("Settings")]
    public float allowedRange = 0.2f;
    public float lerpSpeed = 1f;
    
    private void LateUpdate()
    {
        float x = transform.position.x;

        if (Mathf.Abs(x) > allowedRange)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(x, 0f, Time.deltaTime * lerpSpeed);
            transform.position = pos;
        }
    }
}