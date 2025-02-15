using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public void CargarMenuNiveles()
    {
        SceneManager.LoadScene("MenuNiveles");
    }
}