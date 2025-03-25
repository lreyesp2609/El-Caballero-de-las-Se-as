using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class CreditosManager3 : MonoBehaviour
{
    [Header("Text Configuration")]
    public TextMeshProUGUI creditText;
    public float letterDelay = 0.05f;
    public string nextSceneName = "MenuNiveles";
    public GameObject continuePrompt;

    private string[] creditLines = {
    "¡GRACIAS POR JUGAR!",
    "Recuerda: la inclusión es la clave para un mundo mejor.",
    "Este juego es un homenaje a la diversidad y a la importancia de comunicarnos más allá de las palabras.",
    "¡Sigue explorando, aprendiendo y compartiendo!"
    };

    private bool isTyping = false;
    private bool skipRequested = false;

    void Start()
    {
        continuePrompt.SetActive(false);
        StartCoroutine(ShowCredits());
    }

    IEnumerator ShowCredits()
    {
        for (int i = 0; i < creditLines.Length; i++)
        {
            if (!string.IsNullOrEmpty(creditLines[i]))
            {
                StartCoroutine(TypeText(creditLines[i]));

                yield return new WaitUntil(() => !isTyping);

                continuePrompt.SetActive(true);
                yield return new WaitUntil(() => Input.anyKeyDown);
                continuePrompt.SetActive(false);

                creditText.text += "\n";
            }
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        string initialText = creditText.text; // Guardamos el texto inicial
        string currentText = initialText;

        foreach (char letter in text.ToCharArray())
        {
            if (skipRequested)
            {
                // Usamos initialText para evitar duplicar caracteres
                creditText.text = initialText + text;
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
