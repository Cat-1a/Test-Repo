using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    [Header("抓取设置")]
    public bool canBeGrabbed = true;

    [Header("视觉效果")]
    public Color highlightColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Rigidbody2D rb;

    public bool IsGrabbed { get; private set; }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void OnGrabbed()
    {
        IsGrabbed = true;

        // 禁用物理效果
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }

        // 改变颜色
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlightColor;
        }
    }

    public void OnReleased()
    {
        IsGrabbed = false;

        // 恢复物理效果
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // 恢复颜色
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    // 当玩家靠近时高亮
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canBeGrabbed && !IsGrabbed)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = highlightColor;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsGrabbed)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }
}
