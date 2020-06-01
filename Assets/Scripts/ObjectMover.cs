using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMover : MonoBehaviour
{
    public Transform ObjectTransform { get; set; }

    [SerializeField] private float moveValue;
    [SerializeField] private float rotateValue;
    
    [SerializeField] private Text posXText;
    [SerializeField] private Text posYText;
    [SerializeField] private Text posZText;
    [SerializeField] private Text rotXText;
    [SerializeField] private Text rotYText;
    [SerializeField] private Text rotZText;


    private void Update()
    {
        if (ObjectTransform == null)
        {
            posXText.text = "x:";
            posYText.text = "y:";
            posZText.text = "z:";
            rotXText.text = "x:";
            rotYText.text = "y:";
            rotZText.text = "z:";
            return;
        }

        posXText.text = $"x: {ObjectTransform.localPosition.x}";
        posYText.text = $"y: {ObjectTransform.localPosition.y}";
        posZText.text = $"z: {ObjectTransform.localPosition.z}";
        rotXText.text = $"x: {ObjectTransform.localRotation.x}";
        rotYText.text = $"y: {ObjectTransform.localRotation.y}";
        rotZText.text = $"z: {ObjectTransform.localRotation.z}";
    }



    public void MoveX(int increase)
    {
        ObjectTransform.localPosition += Vector3.right * increase * moveValue;
    }
    
    public void MoveY(int increase)
    {
        ObjectTransform.localPosition += Vector3.up * increase * moveValue;
    }
    
    public void MoveZ(int increase)
    {
        ObjectTransform.localPosition += Vector3.forward * increase * moveValue;
    }
}