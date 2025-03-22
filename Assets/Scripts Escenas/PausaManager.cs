using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaManager : MonoBehaviour
{
    [Header("Configuración de Pausa")]
    [SerializeField] private KeyCode teclaPausa = KeyCode.Escape;
    [SerializeField] private GameObject panelPausa;

    [Header("Otras Teclas")]
    [SerializeField] private KeyCode teclaReiniciar = KeyCode.R;
    [SerializeField] private KeyCode teclaSalirMenu = KeyCode.S;
    [SerializeField] private string nombreMenuPrincipal = "MenuPrincipal"; // Nombre de tu escena de menú

    private bool juegoEnPausa = false;

    void Start()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Tecla de pausa
        if (Input.GetKeyDown(teclaPausa))
        {
            TogglePausa();
        }

        // Solo procesar estas teclas si el juego está en pausa
        if (juegoEnPausa)
        {
            // Tecla para reiniciar nivel
            if (Input.GetKeyDown(teclaReiniciar))
            {
                ReiniciarNivel();
            }

            // Tecla para salir al menú principal
            if (Input.GetKeyDown(teclaSalirMenu))
            {
                SalirAlMenuPrincipal();
            }
        }
    }

    public void TogglePausa()
    {
        juegoEnPausa = !juegoEnPausa;

        Time.timeScale = juegoEnPausa ? 0f : 1f;
        panelPausa.SetActive(juegoEnPausa);

        Cursor.visible = juegoEnPausa;
        Cursor.lockState = juegoEnPausa ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo se reanude
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
        Debug.Log("Nivel reiniciado");
    }

    private void SalirAlMenuPrincipal()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo se reanude
        SceneManager.LoadScene(nombreMenuPrincipal);
        Debug.Log("Volviendo al menú principal");
    }
}