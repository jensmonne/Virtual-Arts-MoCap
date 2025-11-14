using UnityEngine;

[CreateAssetMenu(fileName = "PoseData", menuName = "Scriptable Objects/PoseData")]
public class PoseData : ScriptableObject
{
    public BonePose[] bones;
}

[System.Serializable]
public class BonePose
{
    public string boneName;
    public Vector3 localPosition;
    public Quaternion localRotation;
}