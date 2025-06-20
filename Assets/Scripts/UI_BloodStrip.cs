using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_BloodStrip : MonoBehaviour
{
    public static event System.Action<float> OnHealthDecreaseRequested; // 新增：全局血条减少事件
    
    public Image immediateHealthBar; // 直接减少的血条
    public Image gradualHealthBar; // 逐步减少的血条
    public float decreaseSpeed = 0.1f; // 血条减少的速度

    private float targetFillAmount; // 目标血条填充值

    void Start()
    {
        // 初始化血条填充值
        targetFillAmount = immediateHealthBar.fillAmount = gradualHealthBar.fillAmount = 1.0f; // 初始血条满血
        OnHealthDecreaseRequested += (amount) =>
        {
            DecreaseHealth(amount);
        };
    }

    public void DecreaseHealth(float amount)
    {
        // 触发事件通知
        OnHealthDecreaseRequested?.Invoke(amount);
        
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