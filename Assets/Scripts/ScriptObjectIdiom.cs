    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Idiom", menuName = "ScriptableObjects/ScriptObjectIdiom")]
public class ScriptObjectIdiom : ScriptableObject
{
    public Idiom[] idioms;
}
[Serializable]
public class Idiom
{
    public int id;
    public string name;
    public string description;

    public bool isCorrect;
    public string answer;
}