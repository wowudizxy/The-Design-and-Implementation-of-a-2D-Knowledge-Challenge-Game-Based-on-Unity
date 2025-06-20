using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MathDataManager : MonoBehaviour
{
    public static MathDataManager Instance { get; private set; }
    public ScriptObjectMath mathData;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 新增：防止场景切换时销毁
        }

        // 修改：确保资源加载成功
        mathData = Resources.Load<ScriptObjectMath>("Data/Math");
        if (mathData == null)
        {
            Debug.LogError("Failed to load Math data resource!");
            return;
        }

        LoadMathData();
    }

    List<Math> maths = new List<Math>();
    List<Math> questionMaths = new List<Math>();
    public Dictionary<int, Math> AnswerMaths = new Dictionary<int, Math>();
    public Math GetRandomQuestion()
    {
        if (questionMaths.Count == 0)
        {
            return null; // 如果列表为空则返回null
        }

        int randomIndex = Random.Range(0, questionMaths.Count);
        Math selectedMath = questionMaths[randomIndex];
        questionMaths.RemoveAt(randomIndex);
        return selectedMath;
    }
    public List<Math> GetMathData()
    {
        if (mathData == null || mathData.maths == null)
        {
            Debug.LogWarning("Math data is not loaded properly");
            return new List<Math>();
        }
        return mathData.maths;
    }
    public int GetCorrectAnswerCount()
    {
        if (AnswerMaths == null) return 0;
        return AnswerMaths.Values.Count(math => !math.isCorrect);
    }

    public int GetWrongAnswerCount()
    {
        if (AnswerMaths == null) return 0;
        return AnswerMaths.Values.Count(math => math.isCorrect);
    }

    public void AddAnsweredMathQuestion(Math math)
    {
        // 修改判断条件，检查题目名称是否已经存在
        if (!mathData.maths.Exists(m => m.name == math.name))
        {
            mathData.maths.Add(math);
            SaveMathData();
        }
    }

    public void ClearDailyData()
    {
        string lastClearDate = PlayerPrefs.GetString("LastClearMathDate", "");
        string currentDate = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (lastClearDate != currentDate)
        {
            mathData.maths.Clear();
            SaveMathData();
            PlayerPrefs.SetString("LastClearMathDate", currentDate);
        }
    }

    private void LoadMathData()
    {
        if (PlayerPrefs.HasKey("MathData"))
        {
            string json = PlayerPrefs.GetString("MathData");
            print("Loaded Math data: " + json);
            JsonUtility.FromJsonOverwrite(json, mathData);
        }
    }

    private void SaveMathData()
    {
        string json = JsonUtility.ToJson(mathData);
        PlayerPrefs.SetString("MathData", json);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        ClearDailyData(); // 启动时检查是否需要清空数据
    }
    public List<Math> GetGenerateMathQuestions()
    {
        maths.Clear();
        questionMaths.Clear();
        HashSet<string> generatedQuestions = new HashSet<string>();
        int currentLevel = PlayerPrefs.GetInt("MathLevel", 1);

        for (int i = 0; i < 20; i++)
        {
            Math math = new Math();
            math.id = i + 1; // 为每个问题分配唯一ID并递增
            string question = "";
            int attempts = 0;
            const int maxAttempts = 100;

            do
            {
                switch (currentLevel)
                {
                    case 1: // 加法关卡
                        int a = Random.Range(1, 10);
                        int b = Random.Range(1, 10);
                        question = $"{a} + {b} = ?";
                        math.answer = (a + b).ToString();
                        break;

                    case 2: // 减法关卡
                        a = Random.Range(5, 20);
                        b = Random.Range(1, 5);
                        question = $"{a} - {b} = ?";
                        math.answer = (a - b).ToString();
                        break;

                    case 3: // 乘法关卡
                        a = Random.Range(1, 10);
                        b = Random.Range(1, 10);
                        question = $"{a} × {b} = ?";
                        math.answer = (a * b).ToString();
                        break;

                    case 4: // 除法关卡
                        b = Random.Range(1, 10);
                        int quotient = Random.Range(1, 10); // 随机生成商
                        a = b * quotient; // 确保被除数是除数的整数倍
                        question = $"{a} ÷ {b} = ?";
                        math.answer = quotient.ToString();
                        break;

                    case 5: // 混合运算关卡
                        int operation = Random.Range(0, 4);
                        switch (operation)
                        {
                            case 0:
                                a = Random.Range(1, 10);
                                b = Random.Range(1, 10);
                                question = $"{a} + {b} = ?";
                                math.answer = (a + b).ToString();
                                break;
                            case 1:
                                a = Random.Range(5, 20);
                                b = Random.Range(1, 5);
                                question = $"{a} - {b} = ?";
                                math.answer = (a - b).ToString();
                                break;
                            case 2:
                                a = Random.Range(1, 10);
                                b = Random.Range(1, 10);
                                question = $"{a} × {b} = ?";
                                math.answer = (a * b).ToString();
                                break;
                            case 3:
                                b = Random.Range(1, 10);
                                int an = Random.Range(1, 10); // 随机生成商
                                a = b * an; // 确保被除数是除数的整数倍
                                question = $"{a} ÷ {b} = ?";
                                math.answer = an.ToString();
                                break;
                        }
                        break;

                    default:
                        return maths; // 默认返回当前列表
                }
                attempts++;
                if (attempts >= maxAttempts) break;
            }
            while (generatedQuestions.Contains(question));

            if (attempts < maxAttempts)
            {
                math.name = question;
                generatedQuestions.Add(question);
                maths.Add(math);
                questionMaths.Add(math);
            }
            else
            {
                i--; // 重试当前题目
            }
        }
        return maths;
    }


}