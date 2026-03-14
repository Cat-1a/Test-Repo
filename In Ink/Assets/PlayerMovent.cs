using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("地面检测")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    public bool canJump = true; 
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //自动创建地面检测
        if (groundCheckPoint == null)
        {
            GameObject groundCheck = new GameObject("GroundCheck");
            groundCheck.transform.parent = transform;
            groundCheck.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheckPoint = groundCheck.transform;
        }
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // 检测是否在地面
        CheckGrounded();

        // 跳
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // 移动
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void CheckGrounded()
    {
        // 使用圆形检测判断是否在地面
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position,
                                             groundCheckRadius,
                                             groundLayer);

        // 落地时重置跳跃能力
        if (isGrounded)
        {
            canJump = true;
        }
    }

    // 公共跳跃方法
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        canJump = false;
        Debug.Log("普通跳跃");
    }

    // 用于在编辑器中显示地面检测范围
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
