using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New ObjectInfoUnitData", menuName = "ObjectInfoUnit Data", order = 51)]
    public class ObjectInfoUnitData : ScriptableObject
    {
        public string unitName;
        public AudioClip[] voices;
        public AnimationClip miniatureAnim;
        public bool disableMiniatureAnimOnSecondVoice;
        public float delayBetweenVoices;
        public Sprite infoImage;
        [TextArea(3, 10)] public string[] infoTextStrings;
        public int pointerReferenceId;
        public string[] sizes;
        public AnimationClip objectAnim;
    }
}