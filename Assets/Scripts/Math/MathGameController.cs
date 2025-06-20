using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MathGameController : MonoBehaviour
{
    public static MathGameController Instance { get; private set; }

    private float Blood = 1;
    [SerializeField] private GameObject mathItemPrefab;
    [SerializeField] private Transform selectionArea;
    [SerializeField] private GameObject gameOverPanel; // 新增：游戏失败提示面板
    [SerializeField] private GameObject successPanel;
    [SerializeField] private BloodControl bloodControl;
    [SerializeField] private float reduceBlood = 0.4f;
    private int AnswerCount = 0;
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
        // 确保MathDataManager实例存在
        if (MathDataManager.Instance == null)
        {
            GameObject managerObj = new GameObject("MathDataManager");
            managerObj.AddComponent<MathDataManager>();
        }

        List<Math> maths = MathDataManager.Instance.GetGenerateMathQuestions();
        foreach (Math math in maths)
        {
            GameObject itemObj = Instantiate(mathItemPrefab, selectionArea);
            MathItem item = itemObj.GetComponent<MathItem>();
            item.SetData(math);
        }
    }
    public void IsLevelComplete()
    {
        AnswerCount++;
        // 获取剩余成语数量

        if (AnswerCount >= 20)
        {
            LevelComplete();
        }
    }

    // 新增：关卡完成处理
    private void LevelComplete()
    {
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