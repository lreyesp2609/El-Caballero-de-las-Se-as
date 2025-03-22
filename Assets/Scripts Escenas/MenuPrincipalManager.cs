using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    private bool isPantallaCompleta = true; // Variable para rastrear el estado

    public void CargarMenuExplicacion()
    {
        SceneManager.LoadScene("MenuExplicacion");
    }

    public void CargarCreditos()
    {
        SceneManager.LoadScene("CREDITOS");
    }

    // Método para salir del juego
    public void SalirDelJuego()
    {
        Application.Quit(); // Cierra la aplicación

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Para cerrar en el Editor de Unity
#endif
    }


    // Método para cambiar entre pantalla completa y ventana
    public void CambiarResolucion()
    {
        isPantallaCompleta = !isPantallaCompleta; // Invertir el estado

        // Cambiar el modo de pantalla
        if (isPantallaCompleta)
        {
            // Pantalla completa con la resolución nativa del monitor
            Screen.SetResolution(Screen.currentResolution.width,
                               Screen.currentResolution.height,
                               FullScreenMode.FullScreenWindow);
        }
        else
        {
            // Modo ventana (tamaño personalizable, ejemplo: 1280x720)
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
    }
}