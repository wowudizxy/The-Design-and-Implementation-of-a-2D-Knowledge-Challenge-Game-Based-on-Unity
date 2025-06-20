using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Question : MonoBehaviour
{
    private bool isCompleted = false;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI questionText;
    public Idiom idiom;

    public bool GetIsCompleted()
    {
        return isCompleted;
    }
    public bool CheckAnswer(string answer)
    {
        return idiom != null && answer == idiom.answer;
    }

    // 新增：设置描述文本
    public void AnswerSuccess()
    {
        if (idiom != null && desText != null)
        {
            desText.text = idiom.description;
            questionText.text = idiom.name;
        }
    }

    // 新增：清空描述文本
    public void ClearDescription()
    {
        if (desText != null)
        {
            desText.text = "         请作答";
        }
    }

    void Start()
    {
        UpdateQuest();
    }

    public void UpdateQuest()
    {
        idiom = IdiomManager.Instance.GetRandomIdiomFromQuest();
        if (idiom != null && !string.IsNullOrEmpty(idiom.name))
        {
            // 用下划线替换答案字符
            string displayText = idiom.name.Replace(idiom.answer, "___");
            questionText.text = displayText;
            ClearDescription();
        }
        else
        {
            isCompleted = true;
            questionText.text = "不错不错";
            desText.text = "        继续加油";
        }
    }
}