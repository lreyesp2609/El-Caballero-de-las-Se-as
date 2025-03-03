using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Botones")]
    public Button restartButton;
    public Button menuButton;

    [Header("Escenas")]
    public string mainMenuScene = "MenuPrincipal"; // Nombre exacto de tu escena de menú

    private void Start()
    {
        // Configurar listeners de los botones
        restartButton.onClick.AddListener(RestartLevel);
        menuButton.onClick.AddListener(GoToMainMenu);

        // Desactivar botones inicialmente
        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        // Activar botones cuando termine el fade
        restartButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    private void RestartLevel()
    {
        // Recargar escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenu()
    {
        // Cargar menú principal
        SceneManager.LoadScene(mainMenuScene);
    }
}