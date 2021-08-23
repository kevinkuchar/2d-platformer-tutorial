using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    [SerializeField]
    private int maxLives = 3;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    public CameraShake cameraShake;
    public float spawnDelay = 3.5f;

    public string spawnSoundName = "Spawn";
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string gameOverSoundName = "GameOver";
    
    private static int _remainingLives;
    public static int RemainingLives {
        get { return _remainingLives; }
    }

    [SerializeField]
    private int startingMoney;
    public static int Money;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;
    
    // cache
    private AudioManager audioManager;

    void Start () {
        _remainingLives = maxLives;
        Money = startingMoney;
         
        if (cameraShake == null) {
            Debug.LogError("No CameraShake referenced in GameMaster");
        }

        // caching
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("GameMaster: No AudioManager instance found in this scene.");
        }
    }

    void Awake () {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
        }
    }

    public void EndGame () {
        audioManager.PlaySound (gameOverSoundName);
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true);
    }

    public IEnumerator _RespawnPlayer () {
        audioManager.PlaySound (respawnCountdownSoundName);
        
        yield return new WaitForSeconds(spawnDelay);
        audioManager.PlaySound (spawnSoundName);
        Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(clone, 3f);
    }

    public static void KillPlayer (Player player) {
        Destroy (player.gameObject);
        _remainingLives -= 1;
        if (_remainingLives <= 0) {
            gm.EndGame ();
        } else {
            gm.StartCoroutine (gm._RespawnPlayer ());
        }
    }

    public static void KillEnemy (Enemy enemy) {
        gm._KillEnemy (enemy);
    }

    public void _KillEnemy (Enemy _enemy) {
        audioManager.PlaySound (_enemy.deathSoundName);

        // Add particles
        Transform _clone = (Transform) Instantiate (_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
        Destroy (_clone.gameObject, 5f);
        // Camera shake
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy (_enemy.gameObject);
    }

    private void ToggleUpgradeMenu () {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.U)) {
            ToggleUpgradeMenu();
        }
    }
}
