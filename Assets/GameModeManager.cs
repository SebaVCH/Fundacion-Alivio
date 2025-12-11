using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;
    
    public enum ModoJuego
    {
        Manual,
        Automatico
    }
    
    public ModoJuego modoActual = ModoJuego.Manual;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void IniciarJuegoManual()
    {
        modoActual = ModoJuego.Manual;
        SceneManager.LoadScene("New Scene");
    }
    
    public void IniciarJuegoAutomatico()
    {
        modoActual = ModoJuego.Automatico;
        SceneManager.LoadScene("New Scene");
    }
    
    public bool EsModoAutomatico()
    {
        return modoActual == ModoJuego.Automatico;
    }
}