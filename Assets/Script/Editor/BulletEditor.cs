using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletEditor : Editor
{
    private SerializedObject data;
    private SerializedProperty id;

    private void OnEnable()
    {
        data = new SerializedObject(target);
        id = data.FindProperty("id");
    }

    public override void OnInspectorGUI()
    {
        data.Update();
        EditorGUILayout.PropertyField(id);

        data.ApplyModifiedProperties();
    }
}

