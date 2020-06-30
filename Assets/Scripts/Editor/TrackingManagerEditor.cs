using AR;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TrackingManager))]
    public class TrackingManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (!Application.isPlaying)
                return;
        
            TrackingManager trManager = target as TrackingManager;

            if (trManager == null)
                return;
        
            foreach (Trackable trackable in trManager.Trackables)
            {
                if (GUILayout.Button($"Enable {trackable.ReferenceName}"))
                {
                    trManager.EnableTrackableWithDefaultTransform(trackable);
                }
            }
        }
    }
}