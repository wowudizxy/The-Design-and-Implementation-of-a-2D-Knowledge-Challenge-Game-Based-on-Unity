using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MissionControl : MonoBehaviour
{
    private const string CORRECT_NUM_KEY = "CorrectNum";
    private const string MISSION_COMPLETED_KEY = "MissionCompleted";
    private const string LAST_GENERATE_DATE_KEY = "LastGenerateDate";


    [SerializeField] private GameObject MissionItemPrefab;
    [SerializeField] private Transform MissionItemParent;
    private ScriptObjectMission missionData;
    [SerializeField] private Button BackButton;
    private ScriptObjectIdiom answerRecordData;
    private string lastGenerateDate;

    void Awake()
    {
        BackButton.onClick.AddListener(CloseMission);
        missionData = Resources.Load<ScriptObjectMission>("Data/Mission");
        answerRecordData = Resources.Load<ScriptObjectIdiom>("Data/AnswerRecord");
        // 从PlayerPrefs读取上次生成日期
        lastGenerateDate = PlayerPrefs.GetString(LAST_GENERATE_DATE_KEY, "");

        ShouldRegenerateMissions();
        
        RegenerateMissions();
        
    }

    private void ShouldRegenerateMissions()
    {
        string today = DateTime.Now.ToString("yyyyMMdd");
        if (lastGenerateDate != today)
        {
            lastGenerateDate = today;
            PlayerPrefs.SetInt(CORRECT_NUM_KEY, 0);
            PlayerPrefs.SetInt("CorrectMathNum", 0);
            // 删除所有任务完成标记
            foreach (var mission in missionData.missions)
            {
                PlayerPrefs.DeleteKey(MissionItem.MISSION_COMPLETED_PREFIX + mission.id);
            }
            PlayerPrefs.SetString(LAST_GENERATE_DATE_KEY, today);
        }
    }
    
    private void RegenerateMissions()
    {
        foreach (Transform child in MissionItemParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var mission in missionData.missions)
        {
            if (PlayerPrefs.GetInt(MissionItem.MISSION_COMPLETED_PREFIX + mission.id, 0) == 0)
            {
                GameObject missionItem = Instantiate(MissionItemPrefab, MissionItemParent);
                missionItem.GetComponent<MissionItem>().SetData(mission);
            }
        }
    }

    private void CloseMission()
    {
        if (gameObject.activeSelf)
        {
            SoundManager.Instance.PlaySoundKey();
            gameObject.SetActive(false);
        }
    }
}