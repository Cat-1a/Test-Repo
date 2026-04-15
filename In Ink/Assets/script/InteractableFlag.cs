using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InteractableFlag : MonoBehaviour
{
    [Header("要打开的面板")]
    public GameObject targetPanel;
    [Header("触发标签")]
    public string playerTag = "Player";

    private bool isPanelOpen = false;

    void Start()
    {
        // 初始化：确保面板默认隐藏
        if (targetPanel != null)
            targetPanel.SetActive(false);
    }

    void Update()
    {
        // 面板打开时，按任意键关闭
        if (isPanelOpen && Input.anyKeyDown)
        {
            ClosePanel();
        }
    }

    // 角色触碰旗帜触发
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPanelOpen) return; // 防止重复打开

        if (other.CompareTag(playerTag))
        {
            isPanelOpen = true;
            targetPanel.SetActive(true); // 仅打开面板
        }
    }

    // 关闭面板（按键/按钮通用）
    public void ClosePanel()
    {
        if (!isPanelOpen) return;

        isPanelOpen = false;
        targetPanel.SetActive(false);
    }
}