using UnityEngine;
using System.Collections; // Necesario para IEnumerator

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float attackAnimationDuration = 0.5f; // Duración de la animación de ataque
    [SerializeField] private float hurtAnimationDuration = 0.5f;
    [SerializeField] private float hurtDelay = 0.2f; // Tiempo de espera antes de la animación

    private bool isHurt = false;
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;
    private bool isGrounded;
    private bool isFrozen = false; // Nuevo: Indica si el jugador está congelado

    public void PlayHurtAnimation()
    {
        if (!isHurt) StartCoroutine(HurtRoutine());
    }

    private IEnumerator HurtRoutine()
    {
        isHurt = true;

        // Esperar el delay antes de la animación
        yield return new WaitForSeconds(hurtDelay);

        animator.SetTrigger("Hurt");

        // Esperar duración de la animación
        yield return new WaitForSeconds(hurtAnimationDuration);

        isHurt = false;
    }

    public float GetHurtDuration()
    {
        return hurtDelay + hurtAnimationDuration;
    }




    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    // Método para obtener la duración del ataque
    public float GetAttackDuration()
    {
        return attackAnimationDuration;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Detectar dirección inicial basada en la escala
        facingRight = transform.localScale.x > 0;
    }

    void Update()
    {
        // Si el jugador está congelado, no hacer nada
        if (isFrozen || isHurt)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
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
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
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

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
        animator.ResetTrigger("Hurt");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Jump");
    }

    public void ResetAllTriggers()
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }

    public float GetDeathAnimationLength()
{
    // Asegúrate de que el nombre coincida con tu animación
    RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    foreach(AnimationClip clip in ac.animationClips)
    {
        if(clip.name == "Knight_Death") // Nombre exacto de tu animación
        {
            return clip.length;
        }
    }
    return 1f; // Valor por defecto si no se encuentra
}
}