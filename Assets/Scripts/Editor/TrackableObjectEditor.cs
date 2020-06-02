using System.Collections;
using System.Collections.Generic;
using AR;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackableObjectBehavior))]
public class TrackableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Application.isPlaying)
            return;
        
        if (GUILayout.Button("Enable Behaviour"))
        {
            ((TrackableObjectBehavior) target).EnableBehaviour();
        }
    }
}
