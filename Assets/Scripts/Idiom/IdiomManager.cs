using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdiomManager : MonoBehaviour
{
    
    private static IdiomManager _instance;
    private const int PER_LEVEL_COUNT = 50;
    private const int SELECT_COUNT = 20;
    private int currentLevel = 1;
    
    private List<Idiom> idioms = new List<Idiom>();
    private ScriptObjectIdiom idiomData;
    private ScriptObjectIdiom answerRecordData;
    private string lastRecordDate; // 新增：记录最后保存日期

    private List<Idiom> questIdioms = new List<Idiom>();
    public Dictionary<int, Idiom> AnswerIdioms =  new Dictionary<int, Idiom>();
    public static IdiomManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<IdiomManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<IdiomManager>();
                    singletonObject.name = typeof(IdiomManager).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }
    
    public List<Idiom>GetAnswerIdioms()
    {
        return answerRecordData.idioms.ToList();
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        
        idiomData = Resources.Load<ScriptObjectIdiom>("Data/Idiom");
        string json = JsonUtility.ToJson(idiomData);
        print(json);
        answerRecordData = Resources.Load<ScriptObjectIdiom>("Data/AnswerRecord");
        if (idiomData == null)
        {
            Debug.LogError("Failed to load idiom data asset");
        }
        currentLevel = PlayerPrefs.GetInt("Level", 1);

        LoadAnswerRecords();

        lastRecordDate = PlayerPrefs.GetString("LastRecordDate", "");
        string currentDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        if (lastRecordDate != currentDate && answerRecordData != null)
        {
            answerRecordData.idioms = new Idiom[0];
            lastRecordDate = currentDate;
            PlayerPrefs.SetString("LastRecordDate", currentDate);
            SaveAnswerRecords(); // 新增：保存清空后的记录
        }
    }

    // 获取当前关卡成语
    public List<Idiom> GetCurrentLevelIdioms()
    {
        if (idiomData == null || idiomData.idioms == null)
            return new List<Idiom>();
        int startIndex = (currentLevel - 1) * PER_LEVEL_COUNT;
        int endIndex = Mathf.Min(startIndex + PER_LEVEL_COUNT, idiomData.idioms.Length);

        if (startIndex >= idiomData.idioms.Length)
            return new List<Idiom>();

        // 深拷贝创建levelIdioms列表
        List<Idiom> levelIdioms = idiomData.idioms.Select(i => new Idiom {
            id = i.id,
            name = i.name,
            description = i.description,
            answer = i.answer,
            isCorrect = i.isCorrect
        }).ToList().GetRange(startIndex, endIndex - startIndex);
        
        // 随机抽取不重复的20个成语
        List<Idiom> selectedIdioms = new List<Idiom>();
        while (selectedIdioms.Count < SELECT_COUNT && levelIdioms.Count > 0)
        {
            int randomIndex = Random.Range(0, levelIdioms.Count);
            Idiom selectedIdiom = levelIdioms[randomIndex];
            
            if (!string.IsNullOrEmpty(selectedIdiom.name))
            {
                int charIndex = Random.Range(0, selectedIdiom.name.Length);
                selectedIdiom.answer = selectedIdiom.name[charIndex].ToString();
            }
            questIdioms.Add(selectedIdiom);
            selectedIdioms.Add(selectedIdiom);
            levelIdioms.RemoveAt(randomIndex);
        }

        // 新增打印日志
        foreach (var idiom in selectedIdioms)
        {
            Debug.Log($"成语:{idiom.name}, 答案:{idiom.answer}");
        }
        idioms = selectedIdioms;
        return selectedIdioms;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    /// <summary>
    /// 从questIdioms中随机获取一个成语并从列表中移除
    /// </summary>
    /// <returns>随机选取的成语，如果列表为空则返回null</returns>
    public Idiom GetRandomIdiomFromQuest()
    {
        if (questIdioms == null || questIdioms.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, questIdioms.Count);
        Idiom selectedIdiom = questIdioms[randomIndex];
        questIdioms.RemoveAt(randomIndex);
        return selectedIdiom;
    }

    public int GetCorrectAnswerCount()
    {
        if (AnswerIdioms == null) return 0;
        return AnswerIdioms.Values.Count(idiom => !idiom.isCorrect);
    }

    public int GetWrongAnswerCount()
    {
        if (AnswerIdioms == null) return 0;
        return AnswerIdioms.Values.Count(idiom => idiom.isCorrect);
    }

    // 新增：保存回答记录到PlayerPrefs
    private void SaveAnswerRecords()
    {
        if (answerRecordData == null) return;
        
        string json = JsonUtility.ToJson(new IdiomListWrapper { idioms = answerRecordData.idioms.ToList() });
        PlayerPrefs.SetString("AnswerRecords", json);
        PlayerPrefs.Save();
    }

    // 新增：从PlayerPrefs加载回答记录
    private void LoadAnswerRecords()
    {
        if (answerRecordData == null || !PlayerPrefs.HasKey("AnswerRecords")) return;
        
        string json = PlayerPrefs.GetString("AnswerRecords");
        IdiomListWrapper wrapper = JsonUtility.FromJson<IdiomListWrapper>(json);
        answerRecordData.idioms = wrapper.idioms.ToArray();
    }

    // 新增：添加回答过的成语到记录中
    public void AddAnsweredIdiom(Idiom idiom)
    {
        if (answerRecordData == null || idiom == null) return;

        // 使用HashSet检查是否已存在
        HashSet<int> existingIds = new HashSet<int>(answerRecordData.idioms.Select(i => i.id));
        if (existingIds.Contains(idiom.id)) return;

        // 添加新记录
        List<Idiom> newRecords = answerRecordData.idioms.ToList();
        newRecords.Add(new Idiom
        {
            id = idiom.id,
            name = idiom.name,
            description = idiom.description,
            answer = idiom.answer,
            isCorrect = idiom.isCorrect
        });
        answerRecordData.idioms = newRecords.ToArray();
        SaveAnswerRecords(); // 新增：保存变更

        // 打印所有记录内容
        Debug.Log("当前记录的所有成语:");
        foreach (var recordedIdiom in answerRecordData.idioms)
        {
            Debug.Log($"ID:{recordedIdiom.id}, 成语:{recordedIdiom.name}, 答案:{recordedIdiom.answer}, 是否正确:{recordedIdiom.isCorrect}");
        }

        // 更新记录日期
        lastRecordDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        PlayerPrefs.SetString("LastRecordDate", lastRecordDate);
    }

    // 设置关卡
    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, 5);
    }
}

// 新增：辅助类用于序列化
[System.Serializable]
public class IdiomListWrapper
{
    public List<Idiom> idioms;
}
