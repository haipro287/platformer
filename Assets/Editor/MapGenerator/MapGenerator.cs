using System;
using System.Collections;
using System.Collections.Generic;
using Platformer;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class PhysicalSimulatorList : MonoBehaviour
{
    [HideInInspector] public GameObject[] objectsToSimulate = new GameObject[] { };

    //public getter method
    public GameObject[] GetList()
    {
        return objectsToSimulate;
    }
}

public class PhysicSimulator : EditorWindow
{
    private string mapName;
    private List<GameObject> preFabs;
    private GameObject preFab;

    private const string _helpText = "Cannot find 'Physical Simulation List' component on any GameObject in the scene!";

    private static Vector2 _windowsMinSize = Vector2.one * 500f;
    private static Rect _helpRect = new Rect(0f, 0f, 400f, 100f);
    private static Rect _listRect = new Rect(Vector2.zero, _windowsMinSize);

    private bool _isActive;

    SerializedObject _objectSO = null;
    ReorderableList _listRE = null;

    PhysicalSimulatorList _simulatorList;


    [MenuItem("Tools/PhysicSimulator")]
    public static void OpenWindow()
    {
        GetWindow(typeof(PhysicSimulator));
    }

    // private void OnGUI()
    // {
    //     GUILayout.Label("Generate New Map", EditorStyles.boldLabel);
    //     mapName = EditorGUILayout.TextField("Map name", mapName);
    //
    //     if (GUILayout.Button("Generate Map"))
    //     {
    //         GenerateMap();
    //     }
    // }

    private void OnEnable()
    {
        _simulatorList = FindObjectOfType<PhysicalSimulatorList>();

        if (_simulatorList)
        {
            _objectSO = new SerializedObject(_simulatorList);

            //init list
            _listRE = new ReorderableList(_objectSO, _objectSO.FindProperty("objectsToSimulate"), true,
                true, true, true);

            //handle drawing
            _listRE.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Game Objects");
            _listRE.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2f;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUIContent objectLabel = new GUIContent($"GameObject {index}");
                //the index will help numerate the serialized fields
                EditorGUI.PropertyField(rect, _listRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
            };
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        if (_objectSO == null)
        {
            EditorGUI.HelpBox(_helpRect, _helpText, MessageType.Warning);
            return;
        }
        else if (_objectSO != null)
        {
            _objectSO.Update();
            _listRE.DoList(_listRect);
            _objectSO.ApplyModifiedProperties();
        }

        GUILayout.Space(_listRE.GetHeight() + 30f);
        GUILayout.Label("Please select Game Objects to simulate");
        GUILayout.Space(10f);

        EditorGUILayout.BeginHorizontal();


        GUILayout.Space(30f);

        if (GUILayout.Button("Run Simulation"))
        {
            RunSimulation();
        }

        GUILayout.Space(30f);

        if (GUILayout.Button("Stop Simulation"))
        {
            StopSimulation();
        }

        GUILayout.Space(30f);

        GUILayout.Label(_isActive ? "Simulation Activated!" : "Simulation Deactivated!", EditorStyles.boldLabel);

        EditorGUILayout.EndHorizontal();
    }

    private void RunSimulation()
    {
        _isActive = true;

        foreach (var obj in _simulatorList.GetList())
        {
            if (obj.GetComponent<Rigidbody>() == null)
                obj.AddComponent<Rigidbody>();

            if (obj.GetComponent<MeshRenderer>() == null)
                obj.AddComponent<MeshRenderer>();
        }
    }

    private void StopSimulation()
    {
        _isActive = false;

        foreach (var obj in _simulatorList.GetList())
        {
            var rb = obj.GetComponent<Rigidbody>();
            var mesh = obj.GetComponent<MeshRenderer>();

            if (rb != null)
                DestroyImmediate(rb);

            if (mesh != null)
                DestroyImmediate(mesh);
        }
    }

    private void GenerateMap()
    {
        Debug.Log("Generated!");
    }
}

public class MapGenerator : EditorWindow
{
    private string mapName;
    private List<GameObject> preFabs;
    private GameObject preFab;

    [MenuItem("Tools/Map generator")]
    public static void OpenWindow()
    {
        GetWindow(typeof(MapGenerator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate New Map", EditorStyles.boldLabel);
        mapName = EditorGUILayout.TextField("Map name", mapName);
        preFab = EditorGUILayout.ObjectField("Prefab", preFab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Generate Map"))
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        Debug.Log("Generated!");
    }
}

public class Example
{
    [MenuItem("Examples/Instantiate Selected")]
    static void InstantiatePrefab()
    {
        Selection.activeObject = PrefabUtility.InstantiatePrefab(Selection.activeObject as GameObject);
    }

    [MenuItem("Examples/Instantiate Selected", true)]
    static bool ValidateInstantiatePrefab()
    {
        GameObject go = Selection.activeObject as GameObject;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }
}