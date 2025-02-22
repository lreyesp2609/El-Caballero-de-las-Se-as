using UnityEngine;
using System.Collections;

public class SkeletonHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public SpriteRenderer healthBarSprite;
    public Animator animator;
    public GameObject interactionPanel;
    public float hitAnimationDuration = 0.3f;

    private Vector3 originalHealthBarScale;
    private bool isDead = false;
    private bool isTakingDamage = false;

    void Start()
    {
        currentHealth = maxHealth;
        originalHealthBarScale = healthBarSprite.transform.localScale;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isTakingDamage) return;

        StartCoroutine(HitRoutine(damage));
    }

    IEnumerator HitRoutine(float damage)
    {
        isTakingDamage = true;

        animator.SetTrigger("Hit");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        // Si murió durante el hit, cancelar todo
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
            yield break; // Salir de la corrutina inmediatamente
        }

        yield return new WaitForSeconds(hitAnimationDuration);

        if (currentHealth > 0)
        {
            animator.SetTrigger("BackToIdle");
        }

        isTakingDamage = false;
    }

    void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBarSprite.transform.localScale = new Vector3(
            originalHealthBarScale.x * healthPercentage,
            originalHealthBarScale.y,
            originalHealthBarScale.z
        );
    }

    public IEnumerator Die()
    {
        isDead = true;

        // Cancelar cualquier animación y transiciones
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("BackToIdle");
        animator.SetTrigger("Dead");

        // Forzar la actualización inmediata del animator
        animator.Update(0f);

        // Esperar el tiempo REAL de la animación de muerte
        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deathAnimationLength);

        healthBarSprite.enabled = false;
        interactionPanel.SetActive(false);
        gameObject.SetActive(false);

        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null) player.Unfreeze();
    }
}