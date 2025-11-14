using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class PoseRecorderWindow : EditorWindow
{
    private Transform avatarRoot;
    private Vector2 scroll;

    private List<Transform> allBones = new List<Transform>();
    private List<bool> selected = new List<bool>();
    
    private string saveFolder = "Assets/RecordedPoses";
    private int poseIndex = 1;
    
    private bool visualizeBones = true;

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    [MenuItem("Tools/Pose Recorder")]
    public static void Open()
    {
        GetWindow<PoseRecorderWindow>("Pose Recorder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Pose Recorder", EditorStyles.boldLabel);

        avatarRoot = (Transform)EditorGUILayout.ObjectField("Avatar Root", avatarRoot, typeof(Transform), true);

        GUILayout.Space(10);

        GUILayout.Label("Save Folder", EditorStyles.miniBoldLabel);
        EditorGUILayout.BeginHorizontal();
        saveFolder = EditorGUILayout.TextField(saveFolder);

        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            string folder = EditorUtility.OpenFolderPanel("Select Save Folder", "Assets", "");
            if (!string.IsNullOrEmpty(folder))
            {
                if (folder.StartsWith(Application.dataPath))
                {
                    saveFolder = "Assets" + folder.Substring(Application.dataPath.Length);
                }
                else
                {
                    Debug.LogError("Folder must be inside the Assets folder.");
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("Scan Bones"))
        {
            ScanBones();
        }
        
        visualizeBones = EditorGUILayout.ToggleLeft("Visualize Selected Bones", visualizeBones);

        if (allBones.Count == 0)
            return;

        GUILayout.Space(10);
        scroll = GUILayout.BeginScrollView(scroll);

        for (int i = 0; i < allBones.Count; i++)
        {
            selected[i] = EditorGUILayout.ToggleLeft(allBones[i].name, selected[i]);
            SceneView.RepaintAll();
        }

        GUILayout.EndScrollView();

        GUILayout.Space(10);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Record Pose (Auto Save)"))
        {
            SavePose();
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(10);

        GUILayout.Label($"Next file: Pose_{poseIndex:000}.asset", EditorStyles.helpBox);
    }

    private void ScanBones()
    {
        allBones.Clear();
        selected.Clear();

        foreach (Transform t in avatarRoot.GetComponentsInChildren<Transform>())
        {
            allBones.Add(t);
            selected.Add(false);
        }
    }

    private void SavePose()
    {
        // Ensure folder exists
        if (!AssetDatabase.IsValidFolder(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
            AssetDatabase.Refresh();
        }

        var pose = CreateInstance<PoseData>();
        var list = new List<BonePose>();

        for (int i = 0; i < allBones.Count; i++)
        {
            if (selected[i])
            {
                list.Add(new BonePose()
                {
                    boneName = allBones[i].name,
                    localPosition = allBones[i].localPosition,
                    localRotation = allBones[i].localRotation
                });
            }
        }

        pose.bones = list.ToArray();

        string finalPath = $"{saveFolder}/Pose_{poseIndex:000}.asset";

        AssetDatabase.CreateAsset(pose, finalPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Saved pose → {finalPath}");

        poseIndex++;
    }
    
    private void OnSceneGUI(SceneView scene)
    {
        if (!visualizeBones || avatarRoot == null || allBones.Count == 0)
            return;

        Handles.color = Color.yellow;

        for (int i = 0; i < allBones.Count; i++)
        {
            if (selected[i] && allBones[i] != null)
            {
                Handles.SphereHandleCap(
                    0,
                    allBones[i].position,
                    Quaternion.identity,
                    0.05f,   // size of sphere
                    EventType.Repaint
                );

                Handles.Label(allBones[i].position, allBones[i].name);
            }
        }
    }
}
