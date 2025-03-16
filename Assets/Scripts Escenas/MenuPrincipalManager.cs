using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para interactuar con UI

public class MenuPrincipalManager : MonoBehaviour
{
    // M�todo p�blico para asignar al bot�n
    public void CargarMenuExplicacion()
    {
        SceneManager.LoadScene("MenuExplicacion");
    }

    // M�todo p�blico para asignar al bot�n
    public void CargarCreditos()
    {
        SceneManager.LoadScene("CREDITOS");
    }
}