using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New AudioClip", menuName = "ScriptableObjects/ScriptObjectAudioClip")]
public class ScriptObjectAudioClip : ScriptableObject
{
    public List<AudioClip> audioClips;

    public object Resources { get; internal set; }
}
