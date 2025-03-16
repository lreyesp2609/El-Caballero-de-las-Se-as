using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExplicacionManager : MonoBehaviour
{
    [Header("Text Configuration")]
    public TextMeshProUGUI storyText;
    public float letterDelay = 0.05f;
    public string nextSceneName = "MenuNiveles";
    public GameObject continuePrompt;

    [Header("Audio Configuration")]
    public AudioClip[] audioClips; // Arreglo con los 8 audios
    public AudioSource audioSource;

    private string[] storyParagraphs = {
        "El Caballero de las Se�as",
        "\"El lenguaje es pensamiento. El pensamiento es existencia. Sin lenguaje� no hay humanidad.\"\n",
        "El Reino de Saphyros yace en ruinas. Anta�o, sus habitantes pose�an el mayor tesoro imaginable: el Lenguaje de las Manos, una lengua sagrada que trascend�a la voz y permit�a a todos comunicarse sin barreras. Sus signos flu�an como el viento, transmitiendo historias, conocimientos y emociones con la precisi�n de un arte perdido. Pero esa era termin�.\n",
        "Nadie recuerda c�mo comenz� el desastre. Algunos susurran sobre un pacto roto con las antiguas deidades del conocimiento. Otros aseguran que fue obra de un rey insensato que os� manipular los s�mbolos prohibidos del lenguaje. Pero lo que es cierto es que el castigo fue absoluto:\n",
        "Una sombra maldita se extendi� sobre Saphyros, consumiendo el lenguaje mismo. Las palabras y los signos fueron arrancados de la realidad, dejando a sus habitantes atrapados en un silencio eterno. Sin comunicaci�n, el reino cay� en el caos, y en su lugar, emergieron seres oscuros que se alimentan del olvido y la desesperaci�n.\n",
        "En medio de este colapso, solo un hombre sigue luchando contra la maldici�n: Remnis, el �ltimo caballero del conocimiento. Ha caminado entre los restos de su mundo, ha visto a los antiguos sabios volverse sombras sin pensamiento, ha enfrentado a aquellos que una vez fueron humanos y que ahora custodian, sin saberlo, los fragmentos del lenguaje perdido.\n",
        "Su destino es claro: enfrentarse a la oscuridad, liberar el poder del lenguaje y restaurar la comunicaci�n antes de que el mundo se convierta en un vac�o sin memoria.\n",
        "Pero el camino no es f�cil. Saphyros est� condenado a la descomposici�n, y Remnis deber� cruzar las �ltimas sombras de su reino antes de que todo se hunda en el verdadero abismo..."
    };



    private bool isTyping = false;
    private bool skipRequested = false;

    void Start()
    {
        continuePrompt.SetActive(false);

        // Verificar AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(ShowStory());
    }

    IEnumerator ShowStory()
    {
        for (int i = 0; i < storyParagraphs.Length; i++) // Usar for para indexar
        {
            // Reproducir audio correspondiente
            audioSource.clip = audioClips[i];
            audioSource.Play();

            // Escribir texto
            StartCoroutine(TypeText(storyParagraphs[i]));

            // Esperar hasta que el audio termine Y el texto haya terminado
            yield return new WaitUntil(() => !audioSource.isPlaying && !isTyping);

            // Mostrar prompt y esperar input
            continuePrompt.SetActive(true);
            yield return new WaitUntil(() => Input.anyKeyDown);
            continuePrompt.SetActive(false);

            storyText.text = "";
            yield return new WaitForSeconds(0.2f);
        }

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        storyText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            if (skipRequested)
            {
                storyText.text = text;
                skipRequested = false;
                break;
            }

            storyText.text += letter;
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