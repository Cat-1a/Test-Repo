using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header("抓取设置")]
    public LayerMask grabbableLayer;  
    public float grabRange = 1f;      // 抓取范围
    public KeyCode grabKey = KeyCode.E;

    private PlayerMovement playerMovement;
    private bool isGrabbing = false;
    private GameObject grabbedObject;
    private bool canJumpFromGrab = false;  // 抓时额外跳跃
    private bool hasUsedGrabJump = false;  // 是否已经用过抓取跳跃

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("没有找到PlayerMovement组件！");
        }
    }

    void Update()
    {
        // 抓取动作
        if (Input.GetKeyDown(grabKey))
        {
            if (!isGrabbing)
            {
                TryGrab();
            }
            else
            {
                Release();
            }
        }

        // 从抓取状态跳跃（在空中且抓住物体时的额外跳跃）
        if (isGrabbing && Input.GetButtonDown("Jump") && canJumpFromGrab && !hasUsedGrabJump)
        {
            JumpFromGrab();
        }

        // 如果落地了，重置抓取跳跃标记
        if (playerMovement != null && playerMovement.canJump)
        {
            hasUsedGrabJump = false;
        }
    }

    void TryGrab()
    {
        // 检测周围的可抓取物体
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, grabRange, grabbableLayer);

        if (hitColliders.Length > 0)
        {
            // 获取最近的物体
            float closestDistance = Mathf.Infinity;
            Collider2D closestCollider = null;

            foreach (Collider2D hitCollider in hitColliders)
            {
                // 检查物体是否有GrabbableObject组件
                GrabbableObject grabbable = hitCollider.GetComponent<GrabbableObject>();
                if (grabbable != null && grabbable.canBeGrabbed)
                {
                    float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestCollider = hitCollider;
                    }
                }
            }

            if (closestCollider != null)
            {
                Grab(closestCollider.gameObject);
            }
        }
    }

    void Grab(GameObject obj)
    {
        isGrabbing = true;
        grabbedObject = obj;
        canJumpFromGrab = true;  // 抓住物体时获得一次额外跳跃机会
        hasUsedGrabJump = false;  // 重置抓取跳跃使用标记

        // 通知物体被抓住了
        GrabbableObject grabbable = obj.GetComponent<GrabbableObject>();
        if (grabbable != null)
        {
            grabbable.OnGrabbed();
        }

        Debug.Log("抓住了物体，可以获得一次额外跳跃");
    }

    void Release()
    {
        if (grabbedObject != null)
        {
            // 通知物体被释放了
            GrabbableObject grabbable = grabbedObject.GetComponent<GrabbableObject>();
            if (grabbable != null)
            {
                grabbable.OnReleased();
            }
        }

        isGrabbing = false;
        grabbedObject = null;
        canJumpFromGrab = false;
        hasUsedGrabJump = false;
    }

    void JumpFromGrab()
    {
        // 调用PlayerMovement的Jump方法
        if (playerMovement != null)
        {
            playerMovement.Jump();
            hasUsedGrabJump = true;  // 标记已经用过抓取跳跃
            Debug.Log("从抓取状态跳跃");
        }
    }

    void OnDrawGizmosSelected()
    {
        // 显示抓取范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}
