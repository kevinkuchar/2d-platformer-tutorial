using UnityEngine;

public class PlayerStats: MonoBehaviour {
    public static PlayerStats instance;
    public int maxHealth = 100;
    public float healthRegenRate = 2;
    public float movementSpeed = 10f;
    private int _curHealth;
    public int curHealth {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public void Awake () {
        if (instance == null) {
            instance = this;
        }
    }
} 