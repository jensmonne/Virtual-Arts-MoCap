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
    private bool recordPositionsOnly = false;
    
    private const string selectionAssetPath = "Assets/RecordedPoses/BoneSelection.asset";

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
        
        recordPositionsOnly = EditorGUILayout.ToggleLeft("Record Only Positions (no rotations)", recordPositionsOnly);
        visualizeBones = EditorGUILayout.ToggleLeft("Visualize Selected Bones", visualizeBones);

        GUILayout.Space(10); // keep this here?
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Selection"))
            SaveBoneSelection();

        if (GUILayout.Button("Load Selection"))
            LoadBoneSelection();
        EditorGUILayout.EndHorizontal();
        
        if (allBones.Count == 0)
            return;
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Select All", GUILayout.Width(120)))
        {
            for (int i = 0; i < selected.Count; i++)
                selected[i] = true;

            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Deselect All", GUILayout.Width(120)))
        {
            for (int i = 0; i < selected.Count; i++)
                selected[i] = false;

            SceneView.RepaintAll();
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(5); // ythis here
        
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
        
        if (avatarRoot == null) return;

        foreach (Transform t in avatarRoot.GetComponentsInChildren<Transform>())
        {
            allBones.Add(t);
            selected.Add(false);
        }
    }
    
    private void SaveBoneSelection()
    {
        BoneSelection asset = ScriptableObject.CreateInstance<BoneSelection>();
        List<string> list = new List<string>();

        for (int i = 0; i < allBones.Count; i++)
            if (selected[i])
                list.Add(allBones[i].name);

        asset.selectedBones = list.ToArray();

        Directory.CreateDirectory(Path.GetDirectoryName(selectionAssetPath));

        AssetDatabase.CreateAsset(asset, selectionAssetPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Saved bone selection.");
    }

    private void LoadBoneSelection()
    {
        var asset = AssetDatabase.LoadAssetAtPath<BoneSelection>(selectionAssetPath);
        if (asset == null)
        {
            Debug.LogWarning("No saved selection found.");
            return;
        }

        for (int i = 0; i < allBones.Count; i++)
            selected[i] = System.Array.IndexOf(asset.selectedBones, allBones[i].name) >= 0;

        SceneView.RepaintAll();
        Debug.Log("Loaded bone selection.");
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
            if (!selected[i])
                continue;

            if (recordPositionsOnly)
            {
                list.Add(new BonePose
                {
                    boneName = allBones[i].name,
                    localPosition = allBones[i].localPosition,
                    localRotation = Quaternion.identity
                });
            }
            else
            {
                list.Add(new BonePose
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
