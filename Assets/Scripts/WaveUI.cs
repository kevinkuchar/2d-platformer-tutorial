using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveNumberText;

    private WaveSpawner.SpawnState previousState;

    // Start is called before the first frame update
    void Start()
    {
        if (spawner == null) {
            Debug.Log("WaveUI: No spawner referenced.");
            this.enabled = false;
        }
        if (waveAnimator == null) {
            Debug.Log("WaveUI: No waveAnimator referenced.");
            this.enabled = false;
        }
        if (waveCountdownText == null) {
            Debug.Log("WaveUI: No waveCountdownText referenced.");
            this.enabled = false;
        }
        if (waveNumberText == null) {
            Debug.Log("WaveUI: No waveNumberText referenced.");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (spawner.State) {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI ();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI ();
                break;
        }

        previousState = spawner.State;
    }

    void UpdateCountingUI () {
        if (previousState != WaveSpawner.SpawnState.COUNTING) {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool ("WaveCountdown", true);
        }
        waveCountdownText.text = ((int) spawner.WaveCountDown).ToString();
    }

    void UpdateSpawningUI () {
        if (previousState != WaveSpawner.SpawnState.SPAWNING) {
            waveAnimator.SetBool ("WaveCountdown", false);        
            waveAnimator.SetBool("WaveIncoming", true);
            waveNumberText.text = spawner.NextWave.ToString();
        }
    }


}
