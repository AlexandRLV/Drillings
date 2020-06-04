using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ObjectMover : MonoBehaviour
{
    public int objectId;
    public Transform objectTransform;
    public Text messageText;

    private string url = @"https://drive.google.com/u/0/uc?id=1wYmurjox2RZkGEXeL8IoYuWiTed-NMIw&export=download";


    private void OnEnable()
    {
        //StartCoroutine(LoadDataAndMove());
    }
    
    private IEnumerator LoadDataAndMove()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        Debug.Log("Request created");
        yield return request.SendWebRequest();
        Debug.Log("Request sent");

        try
        {
            string text = request.downloadHandler.text;

            string[] objectPositions = text.Split('&');
            string[] position = objectPositions[objectId].Split(':');

            float x = float.Parse(position[0]);
            float y = float.Parse(position[1]);
            float z = float.Parse(position[2]);
            Debug.Log($"Loaded position: {x}, {y}, {z}");

            objectTransform.localPosition = new Vector3(x, y, z);

            messageText.text = "Position loaded";
        }
        catch (Exception e)
        {
            messageText.text = e.Message;
        }
    }
}