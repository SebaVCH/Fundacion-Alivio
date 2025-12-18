using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelConfiguracion;
    public GameObject panelCreditos;
    
    void Start()
    {
        MostrarMenuPrincipal();
    }
    
    public void MostrarConfiguracion()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguracion.SetActive(false);
        panelCreditos.SetActive(false);
        panelConfiguracion.SetActive(true);
    }
    
    public void MostrarMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelConfiguracion.SetActive(false);
        panelCreditos.SetActive(false);
    }
    
    public void MostrarCreditos()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguracion.SetActive(false);
        panelCreditos.SetActive(true);
    }
    
    public void SalirJuego()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}