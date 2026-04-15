using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 4;

    public float normalSize = 5;
    public float zoomSize = 8;
    private Camera cam;

    void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }

    public void ZoomOut()
    {
        cam.orthographicSize = zoomSize;
    }

    public void ZoomIn()
    {
        cam.orthographicSize = normalSize;
    }
}
