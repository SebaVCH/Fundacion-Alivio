using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [Header("Botones de Modo de Juego")]
    public Button botonJuegoManual;
    public Button botonJuegoAutomatico;

    void OnEnable()
    {
        // Reconectar listeners cada vez que la escena se activa
        ReconectarBotones();
    }

    void Start()
    {
        ReconectarBotones();
    }

    void ReconectarBotones()
    {
        // Limpiar listeners anteriores
        if (botonJuegoManual != null)
        {
            botonJuegoManual.onClick.RemoveAllListeners();
            botonJuegoManual.onClick.AddListener(IniciarModoManual);
        }

        if (botonJuegoAutomatico != null)
        {
            botonJuegoAutomatico.onClick.RemoveAllListeners();
            botonJuegoAutomatico.onClick.AddListener(IniciarModoAutomatico);
        }

        Debug.Log("[MenuButtons] Botones reconectados correctamente");
    }

    void IniciarModoManual()
    {
        Debug.Log("[MenuButtons] Botón MANUAL presionado");
        if (GameModeManager.Instance != null)
        {
            GameModeManager.Instance.IniciarJuegoManual();
        }
        else
        {
            Debug.LogError("[MenuButtons] GameModeManager.Instance es NULL!");
        }
    }

    void IniciarModoAutomatico()
    {
        Debug.Log("[MenuButtons] Botón AUTOMÁTICO presionado");
        if (GameModeManager.Instance != null)
        {
            GameModeManager.Instance.IniciarJuegoAutomatico();
        }
        else
        {
            Debug.LogError("[MenuButtons] GameModeManager.Instance es NULL!");
        }
    }
}