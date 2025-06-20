using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalControl : MonoBehaviour
{
    [SerializeField] MathDataManager mathDataManager;
    [SerializeField] private Button mathBtn;
    [SerializeField] private Button idiomBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private TextMeshProUGUI goldNumText;
    [SerializeField] private TMP_InputField exchangeInputField;
    [SerializeField] private Button confirmExchangeBtn;

    [SerializeField] private Transform DesTransform;
    [SerializeField] private GameObject answerItemPrefab; // 新增：预制体字段
    [SerializeField] private GameObject answerMathItemPrefab;
    void OnEnable()
    {
        UpdateGoldNum();
    }
    void Start()
    {
        backBtn.onClick.AddListener(BackMain);
        confirmExchangeBtn.onClick.AddListener(OnExchangeConfirm);
        
        // 设置按钮初始透明度
        SetButtonTransparency(idiomBtn, 1f);
        SetButtonTransparency(mathBtn, 0.7f);
        
        // 修改：默认生成成语答题记录
        GenerateIdiomAnswerItems();
        
        // 新增：按钮点击事件并添加音效回调
        idiomBtn.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundKey();
            SetButtonTransparency(idiomBtn, 1f);
            SetButtonTransparency(mathBtn, 0.7f);
            GenerateIdiomAnswerItems();
        });
        mathBtn.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundKey();
            SetButtonTransparency(mathBtn, 1f);
            SetButtonTransparency(idiomBtn, 0.7f);
            GenerateMathAnswerItems();
        });
    }

    // 新增：设置按钮透明度方法
    private void SetButtonTransparency(Button button, float alpha)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null) 
        {
            buttonImage.color = new Color(
                buttonImage.color.r, 
                buttonImage.color.g, 
                buttonImage.color.b, 
                alpha
            );
        }
    }

    // 新增：清除现有答题记录
    private void ClearAnswerItems()
    {
        foreach (Transform child in DesTransform)
        {
            Destroy(child.gameObject);
        }
    }

    // 修改：重命名原方法为成语答题记录
    private void GenerateIdiomAnswerItems()
    {
        ClearAnswerItems();
        List<Idiom> answerIdioms = IdiomManager.Instance.GetAnswerIdioms();
        foreach (Idiom idiom in answerIdioms)
        {
            GameObject item = Instantiate(answerItemPrefab, DesTransform);
            AnswerItem answerItem = item.GetComponent<AnswerItem>();
            if (answerItem != null)
            {
                answerItem.SetAnswer(idiom);
            }
        }
    }

    // 新增：生成数学答题记录
    private void GenerateMathAnswerItems()
    {
        ClearAnswerItems();
        List<Math> answerMaths = mathDataManager.GetMathData();
        foreach (Math math in answerMaths)
        {
            GameObject item = Instantiate(answerMathItemPrefab, DesTransform);
            MathAnwserItem answerItem = item.GetComponent<MathAnwserItem>();
            if (answerItem != null)
            {
                answerItem.SetAnswer(math);
            }
        }
    }
    
    private void BackMain()
    {
        if (gameObject.activeSelf)
        {
            SoundManager.Instance.PlaySoundKey();
            gameObject.SetActive(false);
        }
    }

    private void OnExchangeConfirm()
    {
        if(int.TryParse(exchangeInputField.text, out int exchangeAmount))
        {
            int currentCoins = CoinSystem.Instance.GetCurrentCoins();
            if (exchangeAmount > 0 && exchangeAmount <= currentCoins)
            {
                SoundManager.Instance.PlaySoundKey();
                CoinSystem.Instance.SpendCoins(exchangeAmount);
                UpdateGoldNum();
                exchangeInputField.text = "";
            }
        }
    }

    public void UpdateGoldNum()
    {
        goldNumText.text = CoinSystem.Instance.GetCurrentCoins().ToString();
    }
}