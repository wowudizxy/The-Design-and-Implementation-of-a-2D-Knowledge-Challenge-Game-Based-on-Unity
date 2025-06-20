using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image correctImage;
    private Idiom idiomItem;
    private string correctChar;
    [SerializeField] private TextMeshProUGUI text;
    private Vector3 originalPosition;
    private Transform originalParent;
    private TopView topView;
    private float originalFontSize; // 新增：保存原始字体大小
    private Color originalColor;    // 新增：保存原始颜色



    void Start()
    {
        // 运行时获取TopView实例
        if (topView == null) topView = FindFirstObjectByType<TopView>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySoundPickUp();
        text.GetComponent<CanvasGroup>().blocksRaycasts = false;
        originalPosition = text.transform.position;
        originalParent = text.transform.parent;  // 保存原始父对象
        text.transform.SetParent(transform.parent);     // 设置父对象为自身
                                                        // 新增：保存原始字体样式
        originalFontSize = text.fontSize;
        originalColor = text.color;

    }

    public void OnDrag(PointerEventData eventData)
    {

        text.transform.position = eventData.position;
        text.fontSize = originalFontSize * 1.5f; // 放大20%
        text.color = Color.blue;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        text.fontSize = originalFontSize;
        text.color = originalColor;
        GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;
        Question question = null;
        TextMeshProUGUI textComponent = null;

        // 递归查找Question组件
        if (dropTarget != null)
        {
            Transform current = dropTarget.transform;
            while (current != null && question == null)
            {
                question = current.GetComponent<Question>();
                textComponent = current.GetComponent<TextMeshProUGUI>();
                current = current.parent;
            }
        }

        if (question != null)
        {
            if (!question.GetIsCompleted())
            {
                if (question.CheckAnswer(correctChar))
                {
                    correctImage.gameObject.SetActive(true);
                    SoundManager.Instance.PlaySoundCorrect();
                    PlayerPrefs.SetInt("CorrectNum", PlayerPrefs.GetInt("CorrectNum", 0) + 1);
                    IdiomManager.Instance.AnswerIdioms.TryAdd(question.idiom.id, question.idiom);
                    IdiomManager.Instance.AddAnsweredIdiom(question.idiom); // 新增：记录回答的成语
                    CoinSystem.Instance.AddLevelCoins();
                    if (topView != null) topView.UpdateCoinDisplay();
                    question.AnswerSuccess();
                    text.text = null;
                    GameController.Instance.IsLevelComplete();
                    StartCoroutine(UpdateQuestionAfterDelay(question, 2f));
                }
                else
                {
                    SoundManager.Instance.PlaySoundError();
                    question.idiom.isCorrect = true;
                    IdiomManager.Instance.AnswerIdioms.TryAdd(question.idiom.id, question.idiom);
                    IdiomManager.Instance.AddAnsweredIdiom(question.idiom); // 新增：记录回答的成语
                    Debug.Log("回答错误");
                    text.transform.position = originalPosition;
                    GameController.Instance.ReduceBlood();
                }
            }
            else
            {
                text.transform.position = originalPosition;
            }

        }
        else
        {
            text.transform.position = originalPosition;
        }

        text.transform.SetParent(originalParent);
    }

    IEnumerator UpdateQuestionAfterDelay(Question question, float delay)
    {
        yield return new WaitForSeconds(delay);
        question.UpdateQuest();
    }

    public void SetAnswer(Idiom idiom)
    {
        correctChar = idiom.answer;
        text.text = idiom.answer;
        idiomItem = idiom;
    }
}