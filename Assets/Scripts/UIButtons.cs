using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class ClickExample : MonoBehaviour {
    public Button startButton;
    public string pathScene;

	void Start () {
		Button btn = startButton.GetComponent<Button>();
		btn.onClick.AddListener(() => OnStartClick(pathScene));
	}

	void OnStartClick(string pathScene) {
        SceneManager.LoadScene(pathScene);
	}
}