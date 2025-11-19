#if UNITY_EDITOR
using UnityEngine;

public class VRPause : MonoBehaviour
{
    public void PauseNow()
    {
        UnityEditor.EditorApplication.isPaused = true;
        Debug.Log("Editor paused from VR button.");
    }
}
#endif