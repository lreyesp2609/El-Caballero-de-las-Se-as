using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNivelesManager : MonoBehaviour
{
    public void CargarMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void CargarNivel1()
    {
        SceneManager.LoadScene("Nivel 1");
    }
}
