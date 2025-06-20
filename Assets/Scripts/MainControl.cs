using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainControl : MonoBehaviour
{
    
    [SerializeField] private Button ExitBtn;
    [SerializeField] private Button mathBtn;
    [SerializeField] private Button missionBtn;
    [SerializeField] private GameObject missionCenter;
    [SerializeField] private Button personalCenterBtn;
    [SerializeField] private GameObject personalCenter;
    [SerializeField] private Button signInBtn;
    [SerializeField] private TextMeshProUGUI signInText;
    [SerializeField] private Button idiomBtn;
    void Awake()
    {
        UpdateSignInButtonState();
    }
    void Start()
    {
        personalCenterBtn.onClick.AddListener(OnPersonalCenter);
        signInBtn.onClick.AddListener(SignInLogic);
        idiomBtn.onClick.AddListener(EnterIdiom);
        missionBtn.onClick.AddListener(OpenMission);
        mathBtn.onClick.AddListener(EnterMath);
        ExitBtn.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        SoundManager.Instance.PlaySoundKey();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 编辑器模式下停止运行
#else
        Application.Quit(); // 发布后正式退出
#endif
    }

    private void EnterMath()
    {
        SoundManager.Instance.PlaySoundKey();
        CoinSystem.Instance.ResetLevelCoins();
        PlayerPrefs.SetInt("MathLevel", 1);
        SceneManager.LoadScene("Math");
    }

    private void OpenMission()
    {
        SoundManager.Instance.PlaySoundKey();
        if (!missionCenter.activeSelf)
        {
            missionCenter.SetActive(true);
        }
    }

    private void EnterIdiom()
    {
        SoundManager.Instance.PlaySoundKey();
        CoinSystem.Instance.ResetLevelCoins();
        PlayerPrefs.SetInt("Level", 1);
        SceneManager.LoadScene("Idiom");
    }

    private void SignInLogic()
    {
        SoundManager.Instance.PlaySoundGetCoin();
        SignInSystem.Instance.SignIn();
        UpdateSignInButtonState();
        // 新增：通知个人中心更新金币显示
        personalCenter.GetComponent<PersonalControl>().UpdateGoldNum();
    }

    private void UpdateSignInButtonState()
    {
        if (SignInSystem.Instance.HasSignedInToday())
        {
            signInBtn.interactable = false;
            signInText.text = "已签到";
        }
        else
        {
            signInBtn.interactable = true;
            signInText.text = "签到";
        }
    }

    private void OnPersonalCenter()
    {

        if (!personalCenter.activeSelf)
        {
            SoundManager.Instance.PlaySoundKey();
            personalCenter.SetActive(true);
        }
    }
}