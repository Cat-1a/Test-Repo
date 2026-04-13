using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 15f;

    [Header("跳跃设置")]
    public float jumpForce = 18f;
    public float fallMultiplier = 3.5f;
    public float jumpCutMultiplier = 0.5f;
    public LayerMask groundLayer;

    [Header("防卡/贴墙优化")]
    public float wallPushForce = 0.1f;       // 贴墙推离力
    public float wallCheckDistance = 0.15f; // 贴墙检测距离
    public float coyoteTime = 0.1f;         //  coyote跳，落地后0.1秒仍可跳
    public float jumpBufferTime = 0.1f;     // 起跳缓冲，提前按空格也能跳

    [Header("动画组件")]
    public Animator anim;

    [Header("复位设置")]
    public Transform respawnPoint;

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private float horizontalInput;
    private bool isJumping;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 1. 移动动画（实时响应，不卡死）
        anim.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0.1f);

        // 2. Coyote跳 & 起跳缓冲（优化手感，防止贴墙跳不起来）
        bool grounded = isGrounded();
        coyoteTimeCounter = grounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
        jumpBufferCounter = Input.GetKeyDown(KeyCode.Space) ? jumpBufferTime : jumpBufferCounter - Time.deltaTime;

        // 3. 跳跃逻辑
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpBufferCounter = 0;
        }

        // 4. 短跳逻辑
        if (Input.GetKeyUp(KeyCode.Space) && isJumping && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        // 5. 转向
        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
            Flip();

        // 6. 跳跃/下落动画（严格绑定物理状态）
        anim.SetBool("isJumping", !grounded);
        anim.SetBool("isFalling", !grounded && rb.velocity.y < 0);

        // 7. 落地重置跳跃状态
        if (grounded) isJumping = false;
    }

    void FixedUpdate()
    {
        // 1. 先处理贴墙推离（核心：先推离再移动，避免卡墙）
        PushAwayFromWalls();

        // 2. 处理移动速度
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // 【核心防卡】贴墙推离逻辑：只处理水平方向，不影响跳跃
    void PushAwayFromWalls()
    {
        // 检测左右墙
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, groundLayer);

        // 贴墙时轻微推离，彻底解决卡滞
        if (leftHit.collider != null)
        {
            rb.position += Vector2.right * wallPushForce;
        }
        if (rightHit.collider != null)
        {
            rb.position += Vector2.left * wallPushForce;
        }
    }

    // 地面检测（优化版，防止贴墙误判）
    private bool isGrounded()
    {
        float extraHeight = 0.1f;
        // 加宽检测范围，避免贴墙时地面检测失效
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.8f, 0.1f), 0, Vector2.down, extraHeight, groundLayer);
        return hit.collider != null;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RespawnPoint"))
        {
            Debug.Log("碰到复位区域，正在回起点");

            if (respawnPoint != null)
                transform.position = respawnPoint.position;
            else
                transform.position = Vector3.zero;

            if (rb != null)
                rb.velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        // 地面检测可视化
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * 0.05f, new Vector3(0.8f, 0.1f, 1f));

        // 贴墙检测可视化
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallCheckDistance);
    }
}

