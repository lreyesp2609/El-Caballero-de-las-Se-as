using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExplicacionManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public float letterDelay = 0.05f;
    public float paragraphDelay = 2f;
    public string nextSceneName = "MenuNiveles";
    public GameObject continuePrompt;

    private string[] storyParagraphs = {
        "El Caballero de las Señas",
        "\"El lenguaje es pensamiento. El pensamiento es existencia. Sin lenguaje… no hay humanidad.\"\n",
        "El Reino de Saphyros yace en ruinas. Antaño, sus habitantes poseían el mayor tesoro imaginable: el Lenguaje de las Manos, una lengua sagrada que trascendía la voz y permitía a todos comunicarse sin barreras. Sus signos fluían como el viento, transmitiendo historias, conocimientos y emociones con la precisión de un arte perdido. Pero esa era terminó.\n",
        "Nadie recuerda cómo comenzó el desastre. Algunos susurran sobre un pacto roto con las antiguas deidades del conocimiento. Otros aseguran que fue obra de un rey insensato que osó manipular los símbolos prohibidos del lenguaje. Pero lo que es cierto es que el castigo fue absoluto:\n",
        "El lenguaje se fragmentó en 27 ecos malditos, cada uno corrompido en una criatura de pesadilla. Estas entidades, deformadas y hostiles, vagan por la tierra como heraldos del olvido, devorando las mentes de aquellos que intentan recordar. La gente de Saphyros ya no puede comunicarse. No hay palabras, no hay signos, solo un silencio insoportable y eterno.\n",
        "En medio de este colapso, solo un hombre sigue luchando contra la maldición: Remnis, el último caballero del conocimiento. Ha caminado entre los restos de su mundo, ha visto a los antiguos sabios volverse sombras sin pensamiento, ha enfrentado a aquellos que una vez fueron humanos y que ahora custodian, sin saberlo, los fragmentos del lenguaje perdido.\n",
        "Su destino es claro: derrotar a cada una de las 27 entidades, recuperar los signos robados y restaurar el lenguaje antes de que la humanidad se convierta en un eco vacío.\n",
        "Pero el camino no es fácil. Saphyros está condenado a la descomposición, y Remnis deberá cruzar las últimas sombras de su reino antes de que todo se hunda en el verdadero abismo..."
    };

    private bool isTyping = false;
    private bool skipRequested = false;

    void Start()
    {
        continuePrompt.SetActive(false);
        StartCoroutine(ShowStory());
    }

    IEnumerator ShowStory()
    {
        foreach (string paragraph in storyParagraphs)
        {
            StartCoroutine(TypeText(paragraph));

            yield return new WaitWhile(() => isTyping);

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