using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private int AnswerCount = 0;
    private float Blood = 1;
    [SerializeField] private BloodControl bloodControl;
    [SerializeField] private float reduceBlood = 0.4f;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform selectionArea;
    [SerializeField] private GameObject gameOverPanel; // 新增：游戏失败提示面板
    [SerializeField] private GameObject successPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        GenerateIdioms();
    }
    private void GenerateIdioms()
    {
        List<Idiom> idioms = IdiomManager.Instance.GetCurrentLevelIdioms();
        
        foreach(Idiom idiom in idioms)
        {
            GameObject itemObj = Instantiate(itemPrefab,selectionArea);
            Item item = itemObj.GetComponent<Item>();
            item.SetAnswer(idiom);
        }
    }
// 修改：改为检查是否所有问题已回答
    public void IsLevelComplete()
    {
        AnswerCount++;
        // 获取剩余成语数量
        
        if(AnswerCount >=20)
        {
            LevelComplete();
        }
    }

    // 新增：关卡完成处理
    private void LevelComplete()
    {
        SoundManager.Instance.PlaySoundSuccess();
        if (successPanel != null)
        {
            successPanel.SetActive(true);
            // 新增：调用Success脚本更新UI
            Success success = successPanel.GetComponent<Success>();
            if (success != null)
            {
                success.UpdateSuccessUI();
            }
        }
        Debug.Log("关卡完成");
    }

    public void ReduceBlood()
    {
        bloodControl.DecreaseHealth(reduceBlood);
        Blood -= reduceBlood;
        if (Blood <= 0)
        {
            Blood = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        SoundManager.Instance.PlaySoundFail();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Error error = gameOverPanel.GetComponent<Error>();
            if (error != null)
            {
                error.UpdateSuccessUI();
            }
        }
        Debug.Log("游戏失败");
    }
}