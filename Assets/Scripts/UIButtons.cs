using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickExample : MonoBehaviour {
    public Button startButton;
    public string pathScene;
    public bool esModoAutomatico = false; // Nueva variable

    void Start () {
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.RemoveAllListeners(); // IMPORTANTE: Limpiar listeners anteriores
        
        if (esModoAutomatico) {
            btn.onClick.AddListener(OnStartClickAutomatico);
        } else {
            btn.onClick.AddListener(() => OnStartClick(pathScene));
        }
    }

    void OnStartClick(string pathScene) {
        if (GameModeManager.Instance != null) {
            GameModeManager.Instance.IniciarJuegoManual();
        } else {
            SceneManager.LoadScene(pathScene);
        }
    }
    
    void OnStartClickAutomatico() {
        if (GameModeManager.Instance != null) {
            GameModeManager.Instance.IniciarJuegoAutomatico();
        }
    }
}