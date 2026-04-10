using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 30f;
    [Header("跳跃设置")]
    public float jumpForce = 12.0f;
    public LayerMask groundLayer;
    [Header("动画组件")]
    public Animator anim;
    [Header("复位设置")]
    public Transform respawnPoint;
    private Rigidbody2D rb;
    private bool isFacingRingt = true;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //移动获取
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //动画判断
        if(Mathf.Abs(horizontalInput) > 0.1f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        //跳跃
        Jump();
        //转向
        if((horizontalInput > 0 &&  !isFacingRingt) ||(horizontalInput < 0 && isFacingRingt))
        {
            Flip();
        }
        //起跳 & 下落
        bool grounded=isGrounded();
        anim.SetBool("isJumping", !grounded);
        anim.SetBool("isFalling", !grounded & rb.velocity.y < 0);

    }

    void FixedUpdate()
    {
            rb.velocity=new Vector2(horizontalInput*moveSpeed,rb.velocity.y);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private bool isGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f + extraHeight, groundLayer);
        return hit.collider!=null; 
    }

    private void Flip()
    {
        isFacingRingt = !isFacingRingt;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RespawnPoint"))
        {
            if (respawnPoint != null)
                transform.position = respawnPoint.position;
            else
                transform.position = Vector3.zero;

            if (rb != null)
                rb.velocity = Vector2.zero;
        }
    }
}
