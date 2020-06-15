using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugWriter : MonoBehaviour
{
    private static DebugWriter Instance { get; set; }

    private Text text;

    private void Awake()
    {
        Instance = this;
        text = GetComponentInChildren<Text>();
    }


    public static void Write(string message)
    {
        Instance.text.text += message;
        Instance.text.text += "\n";
    }
}