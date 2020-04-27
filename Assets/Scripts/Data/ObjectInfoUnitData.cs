using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New ObjectInfoUnitData", menuName = "ObjectInfoUnit Data", order = 51)]
public class ObjectInfoUnitData : ScriptableObject
{
    public string unitName;
    public AudioClip[] voices;
    public AnimationClip miniatureAnim;
    public Sprite infoImage;
    [TextArea(3, 10)] public string[] infoTextStrings;
    public int pointerReferenceId;
    public AnimationClip objectAnim;
}