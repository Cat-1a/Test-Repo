using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFlagInteraction : MonoBehaviour
{
    [Header("相机设置")]
    public Camera mainCam;
    public float moveSpeed = 2f;
    public float cameraSize = 5f;
    public Vector3 targetCameraPosition;

    [Header("自动关闭距离")]
    public float autoCloseDistance = 3f;

    private Flag currentFlag;
    private bool isInteracting;
    private bool isUIOpen;

    private Vector3 originalCamPos;
    private float originalCamSize;

    void Start()
    {
        if (mainCam != null)
        {
            originalCamPos = mainCam.transform.position;
            originalCamSize = mainCam.orthographicSize;
        }
    }

    void Update()
    {
        FindClosestFlag();

        // 远离旗帜 → 自动关闭
        if (isUIOpen && currentFlag != null)
        {
            float dist = Vector2.Distance(transform.position, currentFlag.transform.position);
            if (dist > autoCloseDistance)
            {
                StopAllCoroutines();
                StartCoroutine(CloseUICoroutine());
            }
        }

        // 按 T 开关
        if (currentFlag != null && !isInteracting)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (!isUIOpen)
                    StartCoroutine(OpenUICoroutine());
                else
                    StartCoroutine(CloseUICoroutine());
            }
        }
    }

    void FindClosestFlag()
    {
        Flag[] flags = FindObjectsOfType<Flag>();
        float minDist = Mathf.Infinity;
        currentFlag = null;

        foreach (var f in flags)
        {
            float d = Vector2.Distance(transform.position, f.transform.position);
            if (d < minDist)
            {
                minDist = d;
                currentFlag = f;
            }
        }
    }

    IEnumerator OpenUICoroutine()
    {
        isInteracting = true;

        if (currentFlag.promptButton != null)
            currentFlag.promptButton.gameObject.SetActive(false);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            mainCam.transform.position = Vector3.Lerp(originalCamPos, targetCameraPosition, t);
            mainCam.orthographicSize = Mathf.Lerp(originalCamSize, cameraSize, t);
            yield return null;
        }

        // 相机到位
        mainCam.transform.position = targetCameraPosition;
        mainCam.orthographicSize = cameraSize;

        // ?? 延时 1 秒，然后再打开 Image
        yield return new WaitForSeconds(1f);

        if (currentFlag != null)
        {
            currentFlag.flagUI.enabled = true;
            isUIOpen = true;
        }

        isInteracting = false;
    }

    IEnumerator CloseUICoroutine()
    {
        isInteracting = true;
        isUIOpen = false;

        if (currentFlag != null)
            currentFlag.flagUI.enabled = false;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, originalCamPos, t);
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, originalCamSize, t);
            yield return null;
        }

        mainCam.transform.position = originalCamPos;
        mainCam.orthographicSize = originalCamSize;

        if (currentFlag != null && currentFlag.promptButton != null)
            currentFlag.promptButton.gameObject.SetActive(true);

        isInteracting = false;
    }
}

