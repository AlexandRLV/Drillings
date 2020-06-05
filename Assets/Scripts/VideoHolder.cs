using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoHolder : MonoBehaviour
{
    public static VideoHolder Instance { get; private set; }
    
    public VideoClip[] clips;

    public List<VideoPlayer> players;


    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        players = new List<VideoPlayer>();
        
        foreach (VideoClip clip in clips)
        {
            GameObject o = new GameObject();
            o.transform.parent = transform;
            VideoPlayer player = o.AddComponent<VideoPlayer>();
            player.clip = clip;
            player.Prepare();
            players.Add(player);
        }
    }
}
