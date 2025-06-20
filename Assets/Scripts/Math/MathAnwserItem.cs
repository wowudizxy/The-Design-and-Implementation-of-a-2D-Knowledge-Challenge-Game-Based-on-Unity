using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MathAnwserItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mathText;
    [SerializeField] private TextMeshProUGUI isCorrectText;

    public void SetAnswer(Math mathItem)
    {
        mathText.text = mathItem.name.Replace("?", mathItem.answer);
        if(mathItem.isCorrect)
        {
            isCorrectText.text = "回答错误";
            isCorrectText.color = Color.red;
        }
        else
        {
            isCorrectText.text = "回答正确"; 
            isCorrectText.color = Color.green;
        }
    }
}
