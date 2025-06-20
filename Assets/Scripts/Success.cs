using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Success : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private TextMeshProUGUI coinNumtext;
    [SerializeField] private TextMeshProUGUI CorrectText;
    [SerializeField] private TextMeshProUGUI ErrorText;
    private TopView topView;
    void Awake()
    {
        if (topView == null) topView = FindFirstObjectByType<TopView>();
        nextLevelButton.onClick.AddListener(NextLevel);
        BackButton.onClick.AddListener(BackMain);
    }

    private void NextLevel()
    {
        SoundManager.Instance.PlaySoundKey();
        if (SceneManager.GetActiveScene().name == "Idiom")
        {
            CoinSystem.Instance.ResetLevelCoins();
            PlayerPrefs.SetInt("Level", IdiomManager.Instance.GetCurrentLevel() + 1);
            SceneManager.LoadScene("Idiom");
        }
        else
        {
            CoinSystem.Instance.ResetLevelCoins();
            PlayerPrefs.SetInt("MathLevel", PlayerPrefs.GetInt("MathLevel", 1) + 1);
            SceneManager.LoadScene("Math");
        }
    }

    private void BackMain()
    {
        SoundManager.Instance.PlaySoundKey();
        SceneManager.LoadScene("Start");
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

            // 新增：第五关后隐藏下一关按钮
            if (IdiomManager.Instance.GetCurrentLevel() >= 5)
            {
                nextLevelButton.gameObject.SetActive(false);
            }
            else
            {
                nextLevelButton.gameObject.SetActive(true);
            }
        }
        else
        {
            CorrectText.text = MathDataManager.Instance.GetCorrectAnswerCount().ToString();
            ErrorText.text = MathDataManager.Instance.GetWrongAnswerCount().ToString();
            coinNumtext.text = CoinSystem.Instance.GetLevelCoins().ToString();
            levelText.text = "第" + PlayerPrefs.GetInt("MathLevel", 1) + "关";

            // 新增：第五关后隐藏下一关按钮
            if (PlayerPrefs.GetInt("MathLevel", 1) >= 5)
            {
                nextLevelButton.gameObject.SetActive(false);
            }
            else
            {
                nextLevelButton.gameObject.SetActive(true);
            }
        }

    }
}