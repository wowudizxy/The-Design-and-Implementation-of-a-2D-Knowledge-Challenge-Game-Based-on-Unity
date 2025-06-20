using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    private int currentCoins = 0;
    
    public int successCoin = 10;
    public int levelCoins = 0;
    // 单例模式
    private static CoinSystem _instance;
    public static CoinSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CoinSystem>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(CoinSystem).Name;
                    _instance = obj.AddComponent<CoinSystem>();
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
        LoadCoins();
    }

    public void ResetLevelCoins()
    {
        levelCoins = 0;
    }
    public void AddLevelCoins()
    {
        AddCoins(successCoin);
        levelCoins += successCoin;
    }
    // 增加金币
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        SaveCoins();
    }

    // 减少金币
    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            SaveCoins();
            return true;
        }
        return false;
    }
    public int GetLevelCoins()
    {
        return levelCoins;
    }
    // 获取当前金币数量
    public int GetCurrentCoins()
    {
        return currentCoins;
    }

    // 保存金币数据
    private void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
        PlayerPrefs.Save();
    }

    // 加载金币数据
    private void LoadCoins()
    {
        currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
    }
}