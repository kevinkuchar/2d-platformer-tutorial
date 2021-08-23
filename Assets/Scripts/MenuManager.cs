using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private string hoverOverSound = "ButtonHover";
    [SerializeField]
    private string pressButtonSound = "ButtonPress";

    AudioManager audioManager;

    void Start () {
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.Log("MenuManager: No AudioManager found");
        }
    }

    public void StartGame () {
        audioManager.PlaySound (pressButtonSound);
        SceneManager.LoadScene ("TheLevel");
    }

    public void QuitGame () {
        audioManager.PlaySound (pressButtonSound);
        Debug.Log ("Application Quit.");
        Application.Quit();
    }

    public void OnMouseOver () {
        audioManager.PlaySound (hoverOverSound);
    }
}
