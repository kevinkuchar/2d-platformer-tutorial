using UnityEngine;
using UnityStandardAssets._2D;
using System.Collections;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
	private PlayerStats stats;
	public int fallBoundary = -20;

	public string deathSoundName = "DeathVoice";
	public string damageSoundDamge = "Grunt";
	

	AudioManager audioManager;

	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start () {
		stats = PlayerStats.instance;
		stats.curHealth = stats.maxHealth;
		if (statusIndicator == null) {
			Debug.LogError ("Player: No status indicator referenced on Player.");
		} else {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}

		// caching
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("Player: No AudioManager instance found in this scene.");
        }

		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		InvokeRepeating("RegenHealth", 1/ stats.healthRegenRate, 1 / stats.healthRegenRate);
	}

	public void DamagePlayer (int damage)
	{
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) {
			audioManager.PlaySound (deathSoundName);
			GameMaster.KillPlayer (this);
		} else {
			audioManager.PlaySound (damageSoundDamge);
		}
		statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
	}

	void Update ()
	{
		if (transform.position.y <= fallBoundary) {
			DamagePlayer(9999999);
		}
	}

	void RegenHealth () {
		stats.curHealth += 1;
		statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
	}

	void OnUpgradeMenuToggle (bool active) {
		Platformer2DUserControl _player = GetComponent<Platformer2DUserControl>();
		if (_player != null) {
			_player.enabled = !active;
		}
		
		Weapon _weapon = GetComponentInChildren<Weapon>();
		if (_weapon != null) {
			_weapon.enabled = !active;
		}
	}

	void OnDestroy () {
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}
}
