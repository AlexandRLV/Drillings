using System;
using UnityEngine;

namespace AR
{
    [Serializable]
    public class Trackable
    {
        public bool IsActive { get; private set; }
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
        [SerializeField] private Vector3 objectRootPosition;
        [SerializeField] private Vector3 objectRootRotation;
        
        private Transform TargetTransform
        {
            get
            {
                if (targetTransform == null)
                    targetTransform = targetObject.transform;

                return targetTransform;
            }
        }
        private Transform ChildTransform
        {
            get
            {
                if (childTransform == null)
                    childTransform = TargetTransform.GetChild(0);

                return childTransform;
            }
        }

        private LayoutController layoutController;
        private Transform objectTransform;
        private Transform targetTransform;
        private Transform childTransform;
        


        public void UpdateObjectTransform(Transform transform)
        {
            objectTransform = transform;
            
            if (IsActive)
                UpdatePositionAndRotation();
        }
        
        public void UpdatePositionAndRotation()
        {
            TargetTransform.position = objectTransform.position;
            TargetTransform.rotation = Quaternion.Euler(0, objectTransform.rotation.eulerAngles.y, 0);

            ChildTransform.localPosition = objectRootPosition;
            ChildTransform.localRotation = Quaternion.Euler(objectRootRotation);
        }

        public void Enable()
        {
            targetObject.SetActive(true);
            IsActive = true;
        }

        public void Disable()
        {
            targetObject.SetActive(false);
            IsActive = false;
        }
    }
}