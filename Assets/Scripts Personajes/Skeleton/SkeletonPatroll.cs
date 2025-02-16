using UnityEngine;

public class SkeletonPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.1f;

    private Transform currentTarget;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool facingRight;
    private bool isInCombat = false; // Indica si está en combate

    void Start()
    {
        currentTarget = pointA;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asegurar que la dirección inicial sea la correcta
        facingRight = !spriteRenderer.flipX;
        animator.SetBool("FacingRight", facingRight);
    }

    void Update()
    {
        // Si está en combate, no se mueve y entra en Idle
        if (isInCombat)
        {
            animator.SetFloat("Speed", 0); // Animación de Idle
            return;
        }

        // Mueve al esqueleto hacia el objetivo actual
        transform.position = Vector2.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        // Comprobar si ha llegado al destino
        if (Mathf.Abs(transform.position.x - currentTarget.position.x) <= stoppingDistance)
        {
            ChangeTarget();
        }

        // Determina la dirección del movimiento
        float moveDirection = (currentTarget.position.x > transform.position.x) ? 1 : -1;

        // Asegurar que la dirección del sprite es correcta
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            Flip();
        }

        // Actualiza el parámetro "Speed" del Animator
        animator.SetFloat("Speed", moveSpeed);
    }

    void ChangeTarget()
    {
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
    }

    void Flip()
    {
        facingRight = !facingRight;
        animator.SetBool("FacingRight", facingRight);

        // Usar flipX en lugar de escalar manualmente
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    // Cuando el Skeleton detecta al Knight, se detiene y entra en Idle
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Knight"))
        {
            isInCombat = true;
            animator.SetFloat("Speed", 0); // Animación de Idle
        }
    }

    // Cuando el Knight se va, el Skeleton sigue patrullando
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Knight"))
        {
            isInCombat = false;
        }
    }
}
