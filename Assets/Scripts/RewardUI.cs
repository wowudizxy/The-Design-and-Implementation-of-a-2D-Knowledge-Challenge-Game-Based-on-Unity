using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Button backBtn;

    void Start()
    {
        backBtn.onClick.AddListener(OnBack);
    }

    private void OnBack()
    {
        gameObject.SetActive(false);
    }

    public void UpdateCoinDisplay(int coin)
    {
        coinText.text = coin.ToString();
    }
}
