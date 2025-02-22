using UnityEngine;

public class SkeletonPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.1f;
    public GameObject interactionPanel; // Referencia al Panel de interacci�n
    public PlayerMovement player;

    private Transform currentTarget;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool facingRight;
    private bool isInCombat = false; // Indica si est� en combate

    void Start()
    {
        currentTarget = pointA;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asegurar que la direcci�n inicial sea la correcta
        facingRight = !spriteRenderer.flipX;
        animator.SetBool("FacingRight", facingRight);

        // Desactivar el panel al inicio
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }
    // En SkeletonPatrol.cs
    public void TriggerDamageEvent()
    {
        // Forzar sincronizaci�n con el da�o del jugador
        player.PlayHurtAnimation();
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Attack") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Hit")) return;

        if (animator.GetBool("IsAttacking")) return;

        // Si est� en combate, no se mueve y entra en Idle
        if (isInCombat)
        {
            animator.SetFloat("Speed", 0); // Animaci�n de Idle
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

        // Determina la direcci�n del movimiento
        float moveDirection = (currentTarget.position.x > transform.position.x) ? 1 : -1;

        // Asegurar que la direcci�n del sprite es correcta
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            Flip();
        }

        // Actualiza el par�metro "Speed" del Animator
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
            animator.SetFloat("Speed", 0); // Animaci�n de Idle

            // Activar el panel de interacci�n
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(true);
            }

            // Congelar al caballero
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Freeze();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Knight"))
        {
            isInCombat = false;

            // Desactivar el panel de interacci�n
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(false);
            }

            // Descongelar al caballero
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Unfreeze();
            }
        }
    }
}