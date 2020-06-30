using System;
using UnityEngine;

namespace AR
{
    [Serializable]
    public class Trackable
    {
        public bool IsActive => targetObject.activeSelf;
        public bool IsFound => objectTransform != null;
        public string ReferenceName => referenceName;
        public Vector3 Position => TargetTransform.position;
        public LayoutController LayoutController
        {
            get
            {
                if (layoutController == null)
                    layoutController = targetObject.GetComponent<LayoutController>();

                return layoutController;
            }
        }
    
        [SerializeField] private string referenceName;
        [SerializeField] private GameObject targetObject;
        
        private Transform TargetTransform
        {
            get
            {
                if (targetTransform == null)
                    targetTransform = targetObject.transform;

                return targetTransform;
            }
        }
        private Transform targetTransform;

        private LayoutController layoutController;
        private Transform objectTransform;
        


        public void UpdateObjectTransform(Transform transform)
        {
            objectTransform = transform;
            UpdatePositionAndRotation();
        }
        
        public void UpdatePositionAndRotation()
        {
            TargetTransform.position = objectTransform.position;
            TargetTransform.rotation = Quaternion.Euler(0, objectTransform.rotation.eulerAngles.y, 0);
        }

        public void Enable()
        {
            targetObject.SetActive(true);
        }

        public void Disable()
        {
            targetObject.SetActive(false);
        }
    }
}