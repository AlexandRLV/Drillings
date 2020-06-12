using System;
using UnityEngine;
using UnityEngine.Video;

namespace Data
{
    [CreateAssetMenu(fileName = "New ObjectInfoUnitData", menuName = "ObjectInfoUnit Data", order = 51)]
    public class ObjectInfoUnitData : ScriptableObject
    {
        public string unitName;
        [Space] public AudioClip[] voices;
        [Space] public AnimationClip miniatureAnim;
        public float delayBetweenVoices;
        public Sprite infoImage;
        public VideoContainer video;
        [Space] public PhotoContainer[] photos;
        [Space] [TextArea(3, 10)] public string[] infoTextStrings;
        [Space] public int[] pointerReferenceIds;
        [Space] public string[] sizes;
        [Space] public AnimationClip objectAnim;
    }

    [Serializable]
    public struct PhotoContainer
    {
        public Sprite photo;
        public float aspectRatio;
    }

    [Serializable]
    public struct VideoContainer
    {
        public VideoClip clip;
        public int clipId;
        public float aspectRatio;
        public RenderTexture renderTexture;
    }
}