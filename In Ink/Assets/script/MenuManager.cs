using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("主菜单界面")]
    public CanvasGroup mainMenuCanvas;
    public Button btnStart;
    public Button btnHelp;
    public Button btnSetting;
    public Button btnExit;

    [Header("说明界面")]
    public CanvasGroup helpCanvas;
    public Button btnHelpBack;

    [Header("设置界面")]
    public CanvasGroup settingCanvas;
    public Slider sliderBrightness;
    public Slider sliderVolume;
    public Text textBrightnessValue;
    public Text textVolumeValue;
    public Button btnSettingBack;

    [Header("游戏控制")]
    public PlayerController playerController; // 拖入你的PlayerController脚本

    private bool isGamePaused = false;

    void Start()
    {
        // 初始化界面状态
        ShowMainMenu();
        HideHelpMenu();
        HideSettingMenu();

        // 绑定主菜单按钮事件
        btnStart.onClick.AddListener(StartGame);
        btnHelp.onClick.AddListener(ShowHelpMenu);
        btnSetting.onClick.AddListener(ShowSettingMenu);
        btnExit.onClick.AddListener(QuitGame);

        // 绑定说明/设置返回按钮事件
        btnHelpBack.onClick.AddListener(ShowMainMenu);
        btnSettingBack.onClick.AddListener(ShowMainMenu);

        // 绑定滑动条事件
        sliderBrightness.onValueChanged.AddListener(OnBrightnessChanged);
        sliderVolume.onValueChanged.AddListener(OnVolumeChanged);

        // 初始化滑动条数值显示
        OnBrightnessChanged(sliderBrightness.value);
        OnVolumeChanged(sliderVolume.value);

        // 游戏启动时暂停，显示主菜单
        PauseGame();

        // 加载保存的设置
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 100f);
        float savedVolume = PlayerPrefs.GetFloat("Volume", 100f);
        sliderBrightness.value = savedBrightness;
        sliderVolume.value = savedVolume;
    }

    void Update()
    {
        // 按ESC键切换菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                // 菜单打开时，ESC关闭菜单，继续游戏
                if (helpCanvas.alpha > 0) HideHelpMenu();
                if (settingCanvas.alpha > 0) HideSettingMenu();
                ShowMainMenu();
                ResumeGame();
            }
            else
            {
                // 游戏进行时，ESC打开主菜单，暂停游戏
                ShowMainMenu();
                PauseGame();
            }
        }
    }

    #region 界面控制
    // 显示主菜单
    void ShowMainMenu()
    {
        mainMenuCanvas.alpha = 1;
        mainMenuCanvas.interactable = true;
        mainMenuCanvas.blocksRaycasts = true;
    }

    // 隐藏主菜单
    void HideMainMenu()
    {
        mainMenuCanvas.alpha = 0;
        mainMenuCanvas.interactable = false;
        mainMenuCanvas.blocksRaycasts = false;
    }

    // 显示说明菜单
    void ShowHelpMenu()
    {
        HideMainMenu();
        helpCanvas.alpha = 1;
        helpCanvas.interactable = true;
        helpCanvas.blocksRaycasts = true;
    }

    // 隐藏说明菜单
    void HideHelpMenu()
    {
        helpCanvas.alpha = 0;
        helpCanvas.interactable = false;
        helpCanvas.blocksRaycasts = false;
    }

    // 显示设置菜单
    void ShowSettingMenu()
    {
        HideMainMenu();
        settingCanvas.alpha = 1;
        settingCanvas.interactable = true;
        settingCanvas.blocksRaycasts = true;
    }

    // 隐藏设置菜单
    void HideSettingMenu()
    {
        settingCanvas.alpha = 0;
        settingCanvas.interactable = false;
        settingCanvas.blocksRaycasts = false;
    }
    #endregion

    #region 游戏控制
    // 开始游戏
    void StartGame()
    {
        HideMainMenu();
        ResumeGame();
    }

    // 暂停游戏（禁用角色控制）
    void PauseGame()
    {
        isGamePaused = true;
        if (playerController != null)
            playerController.enabled = false; // 禁用角色脚本，无法控制
        Time.timeScale = 0; // 暂停游戏时间（可选，根据需求开启）
    }

    // 恢复游戏（启用角色控制）
    void ResumeGame()
    {
        isGamePaused = false;
        if (playerController != null)
            playerController.enabled = true; // 启用角色脚本，恢复控制
        Time.timeScale = 1; // 恢复游戏时间
    }

    // 退出游戏
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    #endregion

    #region 设置功能
    // 亮度设置
    void OnBrightnessChanged(float value)
    {
        textBrightnessValue.text = $"亮度：{Mathf.RoundToInt(value)}";
        // 这里可添加实际亮度调整逻辑，比如调整相机背景亮度、全局亮度等
        // 示例：调整全局亮度（需根据项目需求实现）
        // RenderSettings.ambientIntensity = value / 100f;
        textBrightnessValue.text = $"亮度：{Mathf.RoundToInt(value)}";
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }

    // 声音设置
    void OnVolumeChanged(float value)
    {
        textVolumeValue.text = $"声音：{Mathf.RoundToInt(value)}";
        // 调整全局音量
        AudioListener.volume = value / 100f;
        textVolumeValue.text = $"声音：{Mathf.RoundToInt(value)}";
        AudioListener.volume = value / 100f;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
    #endregion
}
