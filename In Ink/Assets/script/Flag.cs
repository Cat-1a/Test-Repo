using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flag : MonoBehaviour
{
    public Image flagUI;
    public Button promptButton;

    [HideInInspector] public bool isPlayerNear = false;

    void Start()
    {
        if (promptButton != null) promptButton.gameObject.SetActive(false);
        if (flagUI != null) flagUI.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (promptButton != null) promptButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (promptButton != null) promptButton.gameObject.SetActive(false);
        }
    }
}
