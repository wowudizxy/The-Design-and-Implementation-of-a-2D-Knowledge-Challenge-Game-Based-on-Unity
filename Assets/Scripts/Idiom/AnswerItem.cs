using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI idiomText;
    [SerializeField] private TextMeshProUGUI isCorrectText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void SetAnswer(Idiom idiom)
    {
        idiomText.text = idiom.name;
        if(idiom.isCorrect)
        {
            isCorrectText.text = "回答错误";
            isCorrectText.color = Color.red;
        }
        else
        {
            isCorrectText.text = "回答正确"; 
            isCorrectText.color = Color.green;
        }
        descriptionText.text = idiom.description;
    }
}