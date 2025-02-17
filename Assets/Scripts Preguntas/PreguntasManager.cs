using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Necesario para IEnumerator
using System.Collections.Generic;

public class PreguntasManager : MonoBehaviour
{
    public TextMeshProUGUI preguntaText; // Texto de la pregunta
    public Button[] opcionesButtons; // Botones de opciones
    public Image[] imagenesSeñas; // Imágenes de las señas para cada botón
    public GameObject interactionPanel; // Panel de interacción
    public Animator skeletonAnimator; // Animador del esqueleto
    public GameObject skeletonObject; // Objeto del esqueleto

    public Sprite[] abecedarioSprites; // Sprite con las imágenes del abecedario
    public string[] abecedarioLetras; // Letras del abecedario (A, B, C, ...)

    private string letraCorrecta; // Letra correcta para la pregunta actual

    void Start()
    {
        // Inicializar el sistema de preguntas
        GenerarPregunta();
    }

    // Método para generar una pregunta aleatoria
    public void GenerarPregunta()
    {
        // Seleccionar una letra aleatoria del abecedario
        int indiceLetra = Random.Range(0, abecedarioLetras.Length);
        letraCorrecta = abecedarioLetras[indiceLetra];

        // Mostrar la pregunta
        preguntaText.text = $"¿Cuál es la '{letraCorrecta}' en lenguaje de señas?";

        // Asignar opciones a los botones
        AsignarOpciones(indiceLetra);
    }

    // Método para asignar opciones a los botones

    private void AsignarOpciones(int indiceCorrecto)
    {
        List<(string letra, Sprite imagen)> opciones = new List<(string, Sprite)>();

        // Agregar la opción correcta
        opciones.Add((abecedarioLetras[indiceCorrecto], abecedarioSprites[indiceCorrecto]));

        // Agregar opciones incorrectas (aleatorias)
        while (opciones.Count < opcionesButtons.Length)
        {
            int indiceAleatorio = Random.Range(0, abecedarioLetras.Length);
            string opcionAleatoria = abecedarioLetras[indiceAleatorio];
            Sprite imagenAleatoria = abecedarioSprites[indiceAleatorio];

            // Evitar duplicados
            if (!opciones.Exists(o => o.letra == opcionAleatoria))
            {
                opciones.Add((opcionAleatoria, imagenAleatoria));
            }
        }

        // Mezclar las opciones
        opciones = MezclarLista(opciones);

        // Asignar las opciones a los botones
        for (int i = 0; i < opcionesButtons.Length; i++)
        {
            int index = i;
            opcionesButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = opciones[index].letra;
            imagenesSeñas[index].sprite = opciones[index].imagen;
            opcionesButtons[index].onClick.RemoveAllListeners();
            opcionesButtons[index].onClick.AddListener(() => VerificarRespuesta(opciones[index].letra));
        }
    }

    // Método para verificar la respuesta seleccionada
    private void VerificarRespuesta(string respuestaSeleccionada)
    {
        if (respuestaSeleccionada == letraCorrecta)
        {
            Debug.Log("¡Respuesta correcta!");
            StartCoroutine(DisappearAfterAnimation()); // Desaparece el panel y el esqueleto
        }
        else
        {
            Debug.Log("Respuesta incorrecta.");
            GenerarPregunta(); // Solo genera nueva pregunta si la respuesta es incorrecta
        }
    }

    // Corrutina para desaparecer al esqueleto después de la animación
    private IEnumerator DisappearAfterAnimation()
    {
        skeletonAnimator.SetTrigger("Dead");

        // Desactivar el panel al instante
        interactionPanel.SetActive(false);

        // Esperar para la animación de muerte
        yield return new WaitForSeconds(1.0f);

        skeletonObject.SetActive(false);

        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.Unfreeze();
        }
    }

    // Método para mezclar una lista (algoritmo de Fisher-Yates)
    private List<T> MezclarLista<T>(List<T> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = lista[i];
            lista[i] = lista[j];
            lista[j] = temp;
        }
        return lista;
    }
}