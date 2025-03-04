using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public PlayerMovement playerMovement;

    [Header("Paneles de Interacción")]
    public GameObject[] interactionPanels;

    [SerializeField] private float deathAnimationDuration = 1f;

    [Header("Game Over Settings")]
    public CanvasGroup blackScreen;
    public float fadeDuration = 2f;
    public AudioClip deathSound;
    public AudioSource ambientSound;

    private AudioSource audioSource;

    [Header("Sonido de Muerte")]
    public AudioSource deathSoundSource; // AudioSource específico para la muerte (desactivado inicialmente)

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        audioSource = GetComponent<AudioSource>();

        // Configurar pantalla negra inicialmente transparente
        if (blackScreen != null)
        {
            blackScreen.alpha = 0;
            blackScreen.gameObject.SetActive(false);
        }
    }

    public void LoseLife()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateHearts();

            if (currentHealth <= 0)
            {
                StartCoroutine(HandleDeath()); // Cambiamos a una corrutina
            }
        }
    }

    private IEnumerator HandleDeath()
    {
        playerMovement.Freeze();
        playerMovement.PlayDeathAnimation();

        // Detener sonido ambiente
        if (ambientSound != null)
        {
            ambientSound.Stop();
        }

        // Activar sonido de muerte
        float soundDuration = 0f;
        if (deathSoundSource != null && deathSoundSource.clip != null)
        {
            deathSoundSource.gameObject.SetActive(true);
            deathSoundSource.Play();
            soundDuration = deathSoundSource.clip.length;
        }
        else
        {
            Debug.LogError("AudioSource de muerte no configurado");
        }

        // Activar pantalla negra INMEDIATAMENTE
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
            StartCoroutine(FadeInBlackScreen());
        }

        // Ocultar TODOS los paneles de interacción
        if (interactionPanels != null && interactionPanels.Length > 0)
        {
            foreach (GameObject panel in interactionPanels)
            {
                if (panel != null) panel.SetActive(false);
            }
        }

        // Calcular qué dura más: la animación o el sonido
        float deathAnimationLength = playerMovement.GetDeathAnimationLength();
        float waitTime = Mathf.Max(deathAnimationLength, soundDuration);

        // Esperar el tiempo necesario para que AMBOS terminen
        yield return new WaitForSeconds(waitTime);

        // Opcional: Desactivar el sonido si sigue activo
        // if(deathSoundSource != null) deathSoundSource.gameObject.SetActive(false);
    }

    private IEnumerator FadeInBlackScreen()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            blackScreen.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackScreen.alpha = 1;

        // Activar botones
        GameOverManager gameOverUI = blackScreen.GetComponent<GameOverManager>();
        if (gameOverUI != null)
        {
            gameOverUI.ShowButtons();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
            hearts[i].enabled = (i < maxHealth);
        }
    }
}