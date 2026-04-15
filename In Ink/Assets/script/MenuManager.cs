using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("面板")]
    public CanvasGroup ui1Canvas;
    public CanvasGroup ui2Canvas;
    public CanvasGroup ui3Canvas;

    [Header("设置")]
    public Slider sliderVol;
    public Slider sliderBright;
    public TMP_Text textVol;
    public TMP_Text textBright;

    [Header("角色")]
    public PlayerController player;

    void Start()
    {
        // 初始化音量
        sliderVol.value = AudioListener.volume * 100;
        textVol.text = $"声音：{Mathf.RoundToInt(sliderVol.value)}";
        // 初始化亮度
        sliderBright.value = 100;
        textBright.text = $"亮度：{Mathf.RoundToInt(sliderBright.value)}";
    }

    // 开始游戏
    public void StartGame()
    {
        Debug.Log("StartGame 按钮被点击！");
        // 隐藏主菜单
        ui1Canvas.alpha = 0;
        ui1Canvas.interactable = false;
        ui1Canvas.blocksRaycasts = false;
        // 启用角色
        player.enabled = true;
        Time.timeScale = 1;
    }

    // 打开说明面板
    public void OpenUI2()
    {
        ui1Canvas.alpha = 0;
        ui1Canvas.interactable = false;
        ui1Canvas.blocksRaycasts = false;

        ui2Canvas.alpha = 1;
        ui2Canvas.interactable = true;
        ui2Canvas.blocksRaycasts = true;
    }

    // 打开设置面板
    public void OpenUI3()
    {
        ui1Canvas.alpha = 0;
        ui1Canvas.interactable = false;
        ui1Canvas.blocksRaycasts = false;

        ui3Canvas.alpha = 1;
        ui3Canvas.interactable = true;
        ui3Canvas.blocksRaycasts = true;
    }

    // 返回主菜单
    public void ShowUI1()
    {
        ui1Canvas.alpha = 1;
        ui1Canvas.interactable = true;
        ui1Canvas.blocksRaycasts = true;

        ui2Canvas.alpha = 0;
        ui2Canvas.interactable = false;
        ui2Canvas.blocksRaycasts = false;

        ui3Canvas.alpha = 0;
        ui3Canvas.interactable = false;
        ui3Canvas.blocksRaycasts = false;

        // 暂停游戏，角色禁用
        player.enabled = false;
        Time.timeScale = 0;
    }

    // 退出游戏
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // 音量变化
    public void OnVolumeChanged()
    {
        float vol = sliderVol.value;
        textVol.text = $"声音：{Mathf.RoundToInt(vol)}";
        AudioListener.volume = vol / 100f;
    }

    // 亮度变化
    public void OnBrightChanged()
    {
        float bright = sliderBright.value;
        textBright.text = $"亮度：{Mathf.RoundToInt(bright)}";
    }

    // ESC呼出菜单
    void Update()
    {
        if (player.enabled && Input.GetKeyDown(KeyCode.Escape))
        {
            ShowUI1();
        }
    }
}
