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
        "CR�DITOS", // T�tulo principal
        "Equipo de Desarrollo:\n", // Encabezado secci�n
        "Cando Moreno Robinson Rodrigo\nGamboa Macias Kevin Rolando\n", // Nombres 
        "Asignatura: Interacci�n Hombre M�quina\n",
        "Docente: Ing. Erazo Moreta Orlando Ramiro\n",
        "Per�odo Acad�mico: 2024-2025",
        "" // �ltimo elemento vac�o para completar los 8
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
            if (!string.IsNullOrEmpty(creditLines[i])) // Saltar l�neas vac�as
            {
                audioSource.clip = audioClips[i];
                audioSource.Play();

                StartCoroutine(TypeText(creditLines[i]));

                yield return new WaitUntil(() => !audioSource.isPlaying && !isTyping);

                continuePrompt.SetActive(true);
                yield return new WaitUntil(() => Input.anyKeyDown);
                continuePrompt.SetActive(false);

                creditText.text += "\n"; // Agregar salto de l�nea despu�s de cada secci�n
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