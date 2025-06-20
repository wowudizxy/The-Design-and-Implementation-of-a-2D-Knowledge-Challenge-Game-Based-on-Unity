using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Error : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinNumtext;
    [SerializeField] private TextMeshProUGUI CorrectText;
    [SerializeField] private TextMeshProUGUI ErrorText;
    [SerializeField] private Button BackButton;
    private TopView topView;
    void Awake()
    {
        BackButton.onClick.AddListener(OnBackButtonClick);
        if (topView == null) topView = FindFirstObjectByType<TopView>();
    }

    private void OnBackButtonClick()
    {
        BackMain();
    }

    // 新增：更新成功界面UI的方法
    public void UpdateSuccessUI()
    {
         if (SceneManager.GetActiveScene().name == "Idiom")
        {
            CorrectText.text = IdiomManager.Instance.GetCorrectAnswerCount().ToString();
            ErrorText.text = IdiomManager.Instance.GetWrongAnswerCount().ToString();
            coinNumtext.text = CoinSystem.Instance.GetLevelCoins().ToString();
            levelText.text = "第" + IdiomManager.Instance.GetCurrentLevel().ToString() + "关";
        }
        else
        {
            CorrectText.text = MathDataManager.Instance.GetCorrectAnswerCount().ToString();
            ErrorText.text = MathDataManager.Instance.GetWrongAnswerCount().ToString();
            coinNumtext.text = CoinSystem.Instance.GetLevelCoins().ToString();
            levelText.text = "第" + PlayerPrefs.GetInt("MathLevel", 1) + "关";
        }
    }

    private void BackMain()
    {
        SoundManager.Instance.PlaySoundKey();
        SceneManager.LoadScene("Start");
    }
}
