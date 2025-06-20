using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinNumText;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI LevelNumText;
    private int coinNum = 0;
    void Awake()
    {
        UpdateCoinDisplay();
        backButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySoundKey();
            SceneManager.LoadScene("Start");
        });
    }
    public void UpdateCoinDisplay()
    {
        if (SceneManager.GetActiveScene().name == "Idiom")
        {
            coinNum = CoinSystem.Instance.GetLevelCoins();
            coinNumText.text = coinNum.ToString();
            LevelNumText.text = "第" + IdiomManager.Instance.GetCurrentLevel().ToString() + "关";
        }
        else
        {
            coinNum = CoinSystem.Instance.GetLevelCoins();
            coinNumText.text = coinNum.ToString();
            LevelNumText.text = "第" + PlayerPrefs.GetInt("MathLevel", 1) + "关";
        }
        
    }
}
