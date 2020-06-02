using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New ObjectInfoUnitData", menuName = "ObjectInfoUnit Data", order = 51)]
    public class ObjectInfoUnitData : ScriptableObject
    {
        public string unitName;
        [Space]
        public AudioClip[] voices;
        [Space]
        public AnimationClip miniatureAnim;
        public float delayBetweenVoices;
        public Sprite infoImage;
        [Space]
        [TextArea(3, 10)] public string[] infoTextStrings;
        [Space]
        public int[] pointerReferenceIds;
        [Space]
        public string[] sizes;
        [Space]
        public AnimationClip objectAnim;
    }
}