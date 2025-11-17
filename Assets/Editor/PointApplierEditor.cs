using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PointApplier))]
public class PointApplierEditor : Editor
{
    private PoseData poseToApply;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Apply Pose", EditorStyles.boldLabel);

        poseToApply = (PoseData)EditorGUILayout.ObjectField("Pose Asset", poseToApply, typeof(PoseData), false);

        if (poseToApply != null && GUILayout.Button("Apply Points"))
        {
            var pa = (PointApplier)target;
            pa.SpawnPoints(poseToApply);
        }
    }
}