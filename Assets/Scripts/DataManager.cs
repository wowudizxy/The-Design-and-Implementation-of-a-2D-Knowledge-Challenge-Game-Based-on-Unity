using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public TextAsset textAsset;
    [SerializeField] public List<ScriptObjectIdiom> idiomData;

    void Start()
    {
        string json = textAsset.text;
        print(json);
        // 使用JsonUtility的辅助方法来解析JSON数组
        JsonUtility.FromJsonOverwrite(json, idiomData);
    }
}

// 辅助类用于解析JSON数组
[System.Serializable]
public class ListWrapper<T>
{
    public List<T> items;
}