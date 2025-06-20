using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathQuestion : MonoBehaviour
{
    private bool isCompleted = false;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI desText;
    public Math mathItem;
    void Start()
    {
        UpdateQuest();
    }
    public bool GetIsCompleted()
    {
        return isCompleted;
    }

    public bool CheckAnswer(string answer)
    {
        return mathItem != null && answer == mathItem.answer;
    }
    public void UpdateQuest()
    {
        mathItem = MathDataManager.Instance.GetRandomQuestion();
        if (mathItem != null && !string.IsNullOrEmpty(mathItem.name))
        {
            ClearDescription();
            questionText.text = mathItem.name;
        }
        else
        {
            isCompleted = true;
            questionText.text = "不错不错";
            desText.text = "继续加油！";
        }

    }
    public void AnswerSuccess()
    {
        if (mathItem != null && questionText != null)
        {
            desText.text = "回答正确";
            questionText.text = mathItem.name.Replace("?", mathItem.answer);
        }
    }

    // 新增：清空描述文本
    public void ClearDescription()
    {
        if (desText != null)
        {
            desText.text = "请作答";
        }
    }
}
