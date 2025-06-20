using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(fileName = "New Math", menuName = "ScriptableObjects/ScriptObjectMath")]
public class ScriptObjectMath : ScriptableObject
{
    public List<Math> maths;
}
[Serializable]
public class Math
{
    public int id;
    public string name;
    public string description;
    public string answer;
    public bool isCorrect;
    
}
