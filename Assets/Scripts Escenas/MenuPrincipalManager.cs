using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public void CargarMenuExplicacion()
    {
        SceneManager.LoadScene("MenuExplicacion");
    }
}