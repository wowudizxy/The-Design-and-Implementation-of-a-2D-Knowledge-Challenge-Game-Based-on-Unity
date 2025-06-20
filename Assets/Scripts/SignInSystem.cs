using UnityEngine;
using System;

public class SignInSystem : MonoBehaviour
{
    private string lastSignInDate;
    
    // 单例模式
    private static SignInSystem _instance;
    public static SignInSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SignInSystem>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(SignInSystem).Name;
                    _instance = obj.AddComponent<SignInSystem>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadSignInData();
    }

    // 检查今天是否已签到
    public bool HasSignedInToday()
    {
        return lastSignInDate == DateTime.Now.ToString("yyyy-MM-dd");
    }

    // 执行签到
    public void SignIn()
    {
        if (!HasSignedInToday())
        {
            RewardManaer.Instance.UpdateCoinDisplay(10);
            CoinSystem.Instance.AddCoins(10);
            lastSignInDate = DateTime.Now.ToString("yyyy-MM-dd");
            SaveSignInData();
        }
    }

    // 保存签到数据
    private void SaveSignInData()
    {
        PlayerPrefs.SetString("LastSignInDate", lastSignInDate);
        PlayerPrefs.Save();
    }

    // 加载签到数据
    private void LoadSignInData()
    {
        lastSignInDate = PlayerPrefs.GetString("LastSignInDate", "");
    }
}