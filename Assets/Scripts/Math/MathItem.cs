using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MathItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image correctImage;
    [SerializeField] private TextMeshProUGUI AnswerText;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Math mathItem;
    private TopView topView;
    private float originalFontSize; // 新增：保存原始字体大小
    private Color originalColor;    // 新增：保存原始颜色

    void Start()
    {
        // 运行时获取TopView实例
        if (topView == null) topView = FindFirstObjectByType<TopView>();
    }
    public void SetData(Math math)
    {
        mathItem = math;
        AnswerText.text = mathItem.answer;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySoundPickUp();
        AnswerText.GetComponent<CanvasGroup>().blocksRaycasts = false;
        originalPosition = AnswerText.transform.position;
        originalParent = AnswerText.transform.parent;
        AnswerText.transform.SetParent(transform.parent);

        // 新增：保存原始字体样式
        originalFontSize = AnswerText.fontSize;
        originalColor = AnswerText.color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        AnswerText.transform.position = eventData.position;
        // 新增：拖拽时改变字体样式
        AnswerText.fontSize = originalFontSize * 1.5f; // 放大20%
        AnswerText.color = Color.blue;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 新增：还原字体样式
        AnswerText.fontSize = originalFontSize;
        AnswerText.color = originalColor;

        GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;
        MathQuestion mathQuestion = null;


        // 递归查找Question组件
        if (dropTarget != null)
        {
            Transform current = dropTarget.transform;
            while (current != null && mathQuestion == null)
            {
                mathQuestion = current.GetComponent<MathQuestion>();
                current = current.parent;
            }
        }
        if (mathQuestion != null)
        {
            if (!mathQuestion.GetIsCompleted())
            {
                if (mathQuestion.CheckAnswer(mathItem.answer))
                {
                    correctImage.gameObject.SetActive(true);
                    SoundManager.Instance.PlaySoundCorrect();
                    PlayerPrefs.SetInt("CorrectMathNum", PlayerPrefs.GetInt("CorrectMathNum", 0) + 1);
                    MathDataManager.Instance.AnswerMaths.TryAdd(mathQuestion.mathItem.id, mathQuestion.mathItem);
                    MathDataManager.Instance.AddAnsweredMathQuestion(mathQuestion.mathItem); // 新增：记录回答的成语
                    CoinSystem.Instance.AddLevelCoins();
                    if (topView != null) topView.UpdateCoinDisplay();
                    mathQuestion.AnswerSuccess();
                    AnswerText.text = null;
                    MathGameController.Instance.IsLevelComplete();
                    StartCoroutine(UpdateQuestionAfterDelay(mathQuestion, 1f));
                }
                else
                {
                    SoundManager.Instance.PlaySoundError();
                    mathQuestion.mathItem.isCorrect = true;
                    MathDataManager.Instance.AnswerMaths.TryAdd(mathQuestion.mathItem.id, mathQuestion.mathItem);
                    MathDataManager.Instance.AddAnsweredMathQuestion(mathQuestion.mathItem); // 新增：记录回答的成语
                    Debug.Log("回答错误");
                    AnswerText.transform.position = originalPosition;
                    MathGameController.Instance.ReduceBlood();
                }
            }
            else
            {
                AnswerText.transform.position = originalPosition;
            }

        }
        else
        {
            AnswerText.transform.position = originalPosition;
        }

        AnswerText.transform.SetParent(originalParent);
    }

    IEnumerator UpdateQuestionAfterDelay(MathQuestion question, float delay)
    {
        yield return new WaitForSeconds(delay);
        question.UpdateQuest();
    }
}
