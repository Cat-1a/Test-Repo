using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随设置")]
    public Transform target;
    [Header("相机参数")]
    public float smoothSpeed =1f;
    [Header("位置参数")]
    public Vector2 bottomLeftOffset = new Vector2(25f, 15f);
   
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(bottomLeftOffset.x, bottomLeftOffset.y, 0);
        desiredPosition.z = -10f;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
