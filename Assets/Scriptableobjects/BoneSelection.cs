using UnityEngine;

[CreateAssetMenu(fileName = "BoneSelection", menuName = "Scriptable Objects/BoneSelection")]
[System.Serializable]
public class BoneSelection : ScriptableObject
{
    public string[] selectedBones;
}