using UnityEngine;

public class VRPauseEditor : MonoBehaviour
{
    public void PauseNow()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = true;
        Debug.Log("Editor paused from VR button.");
#endif
    }
}