using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManaer : MonoBehaviour
{
    [SerializeField] private RewardUI rewardUI;

    private static RewardManaer _instance;
    public static RewardManaer Instance => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateCoinDisplay(int coin)
    {
        rewardUI.gameObject.SetActive(true);
        rewardUI.UpdateCoinDisplay(coin);
    }
}