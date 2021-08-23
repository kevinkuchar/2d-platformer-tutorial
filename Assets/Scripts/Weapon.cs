using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float effectSpawnRate = 10;
    public float fireRate = 0;
    public int damage = 10;

    // Handle camera shaking
    public float camShakeAmt = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake cameraShake;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    public Transform HitPrefab;
    public LayerMask whatToHit;

    public string weaponShootSound = "DefaultShot";

    float timeToSpawnEffect = 0;
    float timeToFire = 0;
    Transform firePoint;

    // Caching
    AudioManager audioManager;

    // Start is called before the first frame update
    void Awake () {
        firePoint = transform.Find ("FirePoint");
        if (firePoint == null) {
            Debug.LogError ("No firepoint found");
        }
    }

    void Start () {
        cameraShake = GameMaster.gm.GetComponent<CameraShake>();
        if (cameraShake == null) {
            Debug.LogError ("No CameraShake script found on GM object.");
        }
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.Log("Weapon: No AudioManager found");
        }
    }

    // Update is called once per frame
    void Update () {
        if (fireRate == 0) {
            if (Input.GetButtonDown ("Fire1")) {
                Shoot ();
            }
        } else {
            if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
                timeToFire = Time.time + 1/fireRate;
                Shoot ();
            }
        }
    }

    void Shoot () {
        Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        // Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100);
        if (hit.collider != null) {
            // Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) {
                // Debug.Log ("We hit " + hit.collider.name + " and did " + damage + " damage.");
                enemy.DamageEnemy(damage);
            }
        }

        if (Time.time >= timeToSpawnEffect) {
            Vector3 hitPos;
            Vector3 hitNormal;
            if (hit.collider == null) {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3 (9999, 9999, 9999);
            } else {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            StartCoroutine (Effect (hitPos, hitNormal));
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    IEnumerator Effect (Vector3 hitPos, Vector3 hitNormal) {
        Transform trail = (Transform) Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if (lr != null) {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
        Destroy (trail.gameObject, 0.04f);

        if (hitNormal != new Vector3 (9999, 9999, 9999)) {
            Instantiate (HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
        }

        Transform clone = (Transform) Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation);

        clone.parent = firePoint;
        float size = Random.Range(0.4f, 1.0f);
        clone.localScale = new Vector3 (size, size, size);
        yield return new WaitForSeconds(.1f);
        Destroy (clone.gameObject);

        // Shake the camera
        cameraShake.Shake(camShakeAmt, camShakeLength);

        // Play Shoot Sound
        audioManager.PlaySound(weaponShootSound);
    }
}

