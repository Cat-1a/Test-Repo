using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("∏˙ÀÊ…Ë÷√")]
    public Transform target;
    public float smoothSpeed =1f;
    public Vector3 offset = new Vector3(0, 0, 15);

    private void LateUpdate()
    {
        Vector3 targetPot=target.position+offset;
        Vector3 desiredPos = new Vector3(targetPot.x, targetPot.y, -10f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
