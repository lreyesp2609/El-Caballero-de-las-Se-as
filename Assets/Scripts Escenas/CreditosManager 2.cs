using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class CreditosManager2 : MonoBehaviour
{
    [Header("Text Configuration")]
    public TextMeshProUGUI creditText;
    public float letterDelay = 0.05f;
    public string nextSceneName = "MenuNiveles";
    public GameObject continuePrompt;

    private string[] creditLines = {
        "CRÉDITOS",
        "REFERENCIAS:\n",
        "https://itch.io/search?type=games&q=Medieval+Fantasy+Character+Pack\n",
        "https://es.vidnoz.com/texto-a-voz.html\n",
        "https://kartoy.itch.io/32x32grimstone-platformer-tileset\n",
        "https://brullov.itch.io/oak-woods\n",
        "https://szadiart.itch.io/pixel-platformer-castle?download\n",
        "https://jesse-m.itch.io/skeleton-pack\n",
        "https://sventhole.itch.io/hero-knight\n",
        "",
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
