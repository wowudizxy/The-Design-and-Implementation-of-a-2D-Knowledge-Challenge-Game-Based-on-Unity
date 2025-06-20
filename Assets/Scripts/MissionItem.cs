using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItem : MonoBehaviour
{
    public const string MISSION_COMPLETED_PREFIX = "MissionCompleted_";

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Button getButton;

    public void SetData(Mission mission)
    {
        coinText.text = mission.coin.ToString();
        desText.text = mission.description;
        if (mission.Type == 0)
        {
            progressText.text = PlayerPrefs.GetInt("CorrectNum", 0) + "/" + mission.achieveNum.ToString();
            getButton.interactable = (PlayerPrefs.GetInt("CorrectNum", 0) >= mission.achieveNum);
        }
        else
        {
            progressText.text = PlayerPrefs.GetInt("CorrectMathNum", 0) + "/" + mission.achieveNum.ToString();
            getButton.interactable = (PlayerPrefs.GetInt("CorrectMathNum", 0) >= mission.achieveNum);
        }

        getButton.onClick.RemoveAllListeners();
        getButton.onClick.AddListener(() => OnGetButtonClick(mission));
    }

    private void OnGetButtonClick(Mission mission)
    {
        SoundManager.Instance.PlaySoundGetCoin();
        CoinSystem.Instance.AddCoins(mission.coin);
        gameObject.SetActive(false);
        PlayerPrefs.SetInt(MISSION_COMPLETED_PREFIX + mission.id, 1);
        RewardManaer.Instance.UpdateCoinDisplay(mission.coin);
    }
}