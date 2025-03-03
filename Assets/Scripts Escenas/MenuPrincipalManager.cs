using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para interactuar con UI

public class MenuPrincipalManager : MonoBehaviour
{
    // Método público para asignar al botón
    public void CargarMenuExplicacion()
    {
        SceneManager.LoadScene("MenuExplicacion");
    }
}