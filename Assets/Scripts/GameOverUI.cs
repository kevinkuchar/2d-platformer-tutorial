using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private string mouseHoverSound = "ButtonHover";
    [SerializeField]
    private string buttonPressSound = "ButtonPress";
    AudioManager audioManager;

    void Start() {
         // caching
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("GameOverUI: No AudioManager instance found in this scene.");
        }
    }

    public void Quit () {
        Debug.Log ("Application Quit.");
        audioManager.PlaySound (buttonPressSound);
        Application.Quit();
    }

    public void Retry () {
        audioManager.PlaySound (buttonPressSound);
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver () {
        audioManager.PlaySound (mouseHoverSound);
    }
}
