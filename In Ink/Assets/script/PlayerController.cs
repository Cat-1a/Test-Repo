using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 14f;
    public float jumpForce = 18f;
    public float fallMultiplier = 3.8f;

    [Header("检测设置")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    [Header("传送设置")]
    public Transform respawnPoint; // 拖入重生点

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enabled = false;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGrounded();

        anim.SetBool("isRunning", x != 0);
        anim.SetBool("isJumping", !isGrounded);
        anim.SetBool("isFalling", !isGrounded && rb.velocity.y < 0);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        if (x != 0)
            transform.localScale = new Vector3(x, 1, 1);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // ?? 核心：触碰物体S → 瞬间传送（无延迟、无Invoke、无报错）
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("S"))
        {
            // 直接传送
            transform.position = respawnPoint.position;
            rb.velocity = Vector2.zero;
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}