using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoriaManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject enemigo; // Arrastra el enemigo aquí en el inspector
    public CanvasGroup canvasVictoria;
    public TextMeshProUGUI textoHistoria;
    public GameObject promptContinuar;
    public AudioSource sonidoAmbiente; // Nuevo campo para el sonido ambiente

    [Header("Configuración")]
    public float velocidadLetras = 0.05f;
    public string siguienteEscena = "Nivel2";
    public AudioClip[] clipsAudio;
    public AudioSource fuenteAudio;

    [Header("Texto Historia")]
    [TextArea(3, 10)]
    public string[] parrafos = {
        "¡Gracias por liberarme!",
        "No tenía control sobre mis acciones...\nLa oscuridad me poseyó.",
        "Por favor, continúa tu viaje y libera a los demás."
    };

    private bool escribiendo = false;
    private bool saltarTexto = false;

    void Start()
    {
        if (fuenteAudio == null)
        {
            fuenteAudio = gameObject.AddComponent<AudioSource>();
        }

        Debug.Log("VictoriaManager iniciado.");
        canvasVictoria.gameObject.SetActive(false);
        canvasVictoria.alpha = 0;
        canvasVictoria.interactable = false;
        canvasVictoria.blocksRaycasts = false;
        promptContinuar.SetActive(false);
    }

    void Update()
    {
        if (enemigo != null && !enemigo.activeInHierarchy && !canvasVictoria.gameObject.activeSelf)
        {
            Debug.Log("Activando CanvasVictoria...");
            StartCoroutine(MostrarSecuenciaVictoria());
        }

        // Si el usuario presiona una tecla o hace clic, saltar la escritura si está en progreso
        if (Input.anyKeyDown && escribiendo)
        {
            saltarTexto = true;
        }
    }

    IEnumerator MostrarSecuenciaVictoria()
    {
        Debug.Log("Habilitando CanvasVictoria...");
        canvasVictoria.gameObject.SetActive(true);

        // Deshabilitar sonido ambiente
        if (sonidoAmbiente != null)
        {
            sonidoAmbiente.Stop();
        }

        // Efecto fade-in
        float duracionFade = 1f;
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            canvasVictoria.alpha = Mathf.Lerp(0, 1, tiempo / duracionFade);
            tiempo += Time.deltaTime;
            yield return null;
        }

        canvasVictoria.alpha = 1;
        canvasVictoria.interactable = true;
        canvasVictoria.blocksRaycasts = true;

        // Iniciar historia
        yield return StartCoroutine(MostrarHistoria());

        // Esperar input final
        promptContinuar.SetActive(true);
        yield return new WaitUntil(() => Input.anyKeyDown);

        SceneManager.LoadScene(siguienteEscena);
    }

    IEnumerator MostrarHistoria()
    {
        for (int i = 0; i < parrafos.Length; i++)
        {
            promptContinuar.SetActive(false); // Asegurar que esté oculto antes de empezar a escribir

            // Reproducir audio
            if (i < clipsAudio.Length && clipsAudio[i] != null)
            {
                fuenteAudio.clip = clipsAudio[i];
                fuenteAudio.Play();
            }

            // Escribir texto
            yield return StartCoroutine(EscribirTexto(parrafos[i]));

            // Esperar a que termine el audio o el usuario presione una tecla
            yield return new WaitUntil(() => !fuenteAudio.isPlaying || Input.anyKeyDown);

            // Mostrar el prompt
            promptContinuar.SetActive(true);
            Debug.Log("Prompt activado.");

            // Esperar al input para avanzar al siguiente párrafo
            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        // Cargar escena después de todos los párrafos
        SceneManager.LoadScene(siguienteEscena);
    }

    IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        textoHistoria.text = "";

        foreach (char letra in texto.ToCharArray())
        {
            if (saltarTexto)
            {
                textoHistoria.text = texto;
                break;
            }

            textoHistoria.text += letra;
            yield return new WaitForSeconds(velocidadLetras);
        }

        escribiendo = false;
        saltarTexto = false;
    }
}
