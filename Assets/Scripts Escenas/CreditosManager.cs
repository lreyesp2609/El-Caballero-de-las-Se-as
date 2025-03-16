using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditosManager : MonoBehaviour
{
    [Header("Text Configuration")]
    public TextMeshProUGUI creditText; // Cambiado de storyText a creditText
    public float letterDelay = 0.05f;
    public string nextSceneName = "MenuNiveles";
    public GameObject continuePrompt;

    [Header("Audio Configuration")]
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private string[] creditLines = {
        "CRÉDITOS", // Título principal
        "Equipo de Desarrollo:\n", // Encabezado sección
        "Cando Moreno Robinson Rodrigo\nGamboa Macias Kevin Rolando\n", // Nombres 
        "Asignatura: Interacción Hombre Máquina\n",
        "Docente: Ing. Erazo Moreta Orlando Ramiro\n",
        "Período Académico: 2024-2025",
        "" // Último elemento vacío para completar los 8
    };

    private bool isTyping = false;
    private bool skipRequested = false;

    void Start()
    {
        continuePrompt.SetActive(false);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(ShowCredits());
    }

    IEnumerator ShowCredits()
    {
        for (int i = 0; i < creditLines.Length; i++)
        {
            if (!string.IsNullOrEmpty(creditLines[i])) // Saltar líneas vacías
            {
                audioSource.clip = audioClips[i];
                audioSource.Play();

                StartCoroutine(TypeText(creditLines[i]));

                yield return new WaitUntil(() => !audioSource.isPlaying && !isTyping);

                continuePrompt.SetActive(true);
                yield return new WaitUntil(() => Input.anyKeyDown);
                continuePrompt.SetActive(false);

                creditText.text += "\n"; // Agregar salto de línea después de cada sección
            }
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        string currentText = creditText.text;

        foreach (char letter in text.ToCharArray())
        {
            if (skipRequested)
            {
                creditText.text = currentText + text;
                skipRequested = false;
                break;
            }

            currentText += letter;
            creditText.text = currentText;
            yield return new WaitForSeconds(letterDelay);
        }

        isTyping = false;
    }

    void Update()
    {
        if (Input.anyKeyDown && isTyping)
        {
            skipRequested = true;
        }
    }
}