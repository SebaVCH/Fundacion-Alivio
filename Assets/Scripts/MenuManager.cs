using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelConfiguracion;
    
    void Start()
    {
        MostrarMenuPrincipal();
    }
    
    public void MostrarConfiguracion()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguracion.SetActive(true);
    }
    
    public void MostrarMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelConfiguracion.SetActive(false);
    }
    
    public void SalirJuego()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}