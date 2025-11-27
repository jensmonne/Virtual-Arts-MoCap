using UnityEngine;

public class LockX : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = 0f;
        transform.position = pos;
    }
}