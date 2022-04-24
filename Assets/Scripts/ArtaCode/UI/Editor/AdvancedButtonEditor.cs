using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(AdvancedButton))]
public class AdvancedButtonEditor : ButtonEditor
{

    public override void OnInspectorGUI()
    {
      

        base.OnInspectorGUI();

        AdvancedButton myTarget = (AdvancedButton)target;

        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onEnter"), true);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onExit"), true);

        this.serializedObject.ApplyModifiedProperties();
    }
}
