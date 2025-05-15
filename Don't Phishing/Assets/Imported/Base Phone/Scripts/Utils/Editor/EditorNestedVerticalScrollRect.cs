using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(NestedVerticalScrollRect))]
public class EditorNestedVerticalScrollRect : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // 기본 인스펙터 표시

        NestedVerticalScrollRect scrollRect = (NestedVerticalScrollRect)target;
        serializedObject.Update();

        // 직접 속성 표시
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ScrollRect"));

        serializedObject.ApplyModifiedProperties();
    }
}
