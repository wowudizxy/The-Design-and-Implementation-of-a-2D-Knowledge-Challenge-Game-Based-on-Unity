using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "ScriptableObjects/ScriptObjectMission")] 
public class ScriptObjectMission : ScriptableObject
{
    public List<Mission> missions;
}
[Serializable]
public class Mission
{
    public int Type;
    public int id;
    public string description;

    public bool isCorrect;
    public int coin;

    public int achieveNum;
}