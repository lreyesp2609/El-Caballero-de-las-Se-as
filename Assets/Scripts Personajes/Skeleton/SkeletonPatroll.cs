using UnityEngine;
using System.Collections; // NECESARIO para usar IEnumerator

public class SkeletonPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.1f;
    public GameObject interactionPanel; // Referencia al Panel de interacción

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

        // Desactivar el panel al inicio
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
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

            // Activar el panel de interacción
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

            // Desactivar el panel de interacción
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

    public void Disappear()
    {
        // Activar la animación de "dead"
        animator.SetTrigger("Dead");

        // Desactivar el objeto después de un pequeño retraso (opcional)
        StartCoroutine(DisappearAfterAnimation());
    }


    private IEnumerator DisappearAfterAnimation()
    {
        // Esperar un momento para que la animación de "dead" se reproduzca
        yield return new WaitForSeconds(1.0f); // Ajusta el tiempo según la duración de la animación

        // Desactivar el esqueleto
        gameObject.SetActive(false);

        // Desactivar el panel de interacción (si está activo)
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }

        // Descongelar al caballero (si está congelado)
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>(); // MÉTODO ACTUALIZADO
        if (player != null)
        {
            player.Unfreeze();
        }
    }
}