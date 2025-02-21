using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;
    private bool isGrounded;
    private bool isFrozen = false; // Nuevo: Indica si el jugador está congelado

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Si el jugador está congelado, no hacer nada
        if (isFrozen)
        {
            rb.linearVelocity = Vector2.zero; // Congelar el movimiento
            animator.SetFloat("Speed", 0); // Congelar la animación
            return;
        }

        // Detección del suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);

        // Movimiento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.ResetTrigger("Jump");
            animator.SetTrigger("Jump");
        }

        // Parámetros para animaciones de salto y caída
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetFloat("AirSpeedY", rb.linearVelocity.y); // Nueva línea: Velocidad vertical

        // Voltear sprite
        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Método para congelar al jugador
    public void Freeze()
    {
        isFrozen = true;
    }

    // Método para descongelar al jugador
    public void Unfreeze()
    {
        isFrozen = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}