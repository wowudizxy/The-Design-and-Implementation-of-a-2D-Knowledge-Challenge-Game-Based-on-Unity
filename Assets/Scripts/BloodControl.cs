using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodControl : MonoBehaviour
{

    [SerializeField]private Image immediateHealthBar; // 直接减少的血条
    [SerializeField]private Image gradualHealthBar; // 逐步减少的血条
    [SerializeField]private float decreaseSpeed = 0.1f; // 血条减少的速度

    private float targetFillAmount; // 目标血条填充值


    void Start()
    {
        // 初始化血条填充值
        targetFillAmount = immediateHealthBar.fillAmount = gradualHealthBar.fillAmount = 1.0f; // 初始血条满血

    }

    public void DecreaseHealth(float amount)
    {
        // 直接减少血条
        immediateHealthBar.fillAmount -= amount;
        targetFillAmount = immediateHealthBar.fillAmount;

        // 启动协程逐步减少血条
        StartCoroutine(GradualDecrease());
    }

    private IEnumerator GradualDecrease()
    {
        // 逐步减少血条直到与直接减少的血条对齐
        while (gradualHealthBar.fillAmount > targetFillAmount)
        {
            gradualHealthBar.fillAmount = Mathf.MoveTowards(gradualHealthBar.fillAmount, targetFillAmount, decreaseSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
