using UnityEngine;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1;
    [Range(0f, 0.5f)]
    public float volumeVariance = 0.1f;
    [Range(0f, 0.5f)]
    public float pitchVariable = 0.1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource (AudioSource _source) {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play () {
        source.volume = volume * (1 + Random.Range(-volumeVariance / 2f, volumeVariance / 2));
        source.pitch = pitch * (1 + Random.Range(-pitchVariable / 2f, pitchVariable / 2));;
        source.Play();
    }

    public void Stop () {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{   
    public static AudioManager instance;

    [SerializeField] 
    Sound[] sounds;

    void Awake () {
        if (instance != null) {
            if (instance != this) {
                Destroy (this.gameObject);
                Debug.LogError ("More than one AudioManager in this scene.");
            }
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start () {
        for (int i = 0; i< sounds.Length; i++) {
            GameObject _go = new GameObject ("Sounds_" + i + "_" + sounds[i].name);
            _go.transform.SetParent (this.transform);
            sounds[i].SetSource (_go.AddComponent<AudioSource>());
        }
        PlaySound ("Music");
    }

    public void PlaySound (string _name) {
        for (int i = 0; i< sounds.Length; i++) {
            if (sounds[i].name == _name) {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning ("AudioManager: Sound not found in list: " + _name);
    }

    public void StopSound (string _name) {
        for (int i = 0; i< sounds.Length; i++) {
            if (sounds[i].name == _name) {
                sounds[i].Stop();
                return;
            }
        }

        Debug.LogWarning ("AudioManager: Sound not found in list: " + _name);
    }
}
