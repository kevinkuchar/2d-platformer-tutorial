using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class ArmRotation : MonoBehaviour
{
    public int rotationOffset = 90;
    public GameObject player;
    private PlatformerCharacter2D platformerCharacter2D;

    void Awake () {
        platformerCharacter2D = player.GetComponent<PlatformerCharacter2D> ();
    }

    void FlipArm (Vector3 armScale)
    {
        armScale.y *= -1;
        transform.localScale = armScale;
    }

    void ApplyRotation (float rotationZ)
    {
        transform.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset);
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 armScale = transform.localScale;
        Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
        float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
        bool isFacingRight = platformerCharacter2D.m_FacingRight;
        difference.Normalize ();

        ApplyRotation (rotationZ);
        if (Math.Abs (rotationZ) <= 90 && isFacingRight) {
            if (isFacingRight && armScale.y <= 0) {
                FlipArm (armScale);
            }
        } else if (Math.Abs (rotationZ) > 90 && !isFacingRight) {
            // ApplyRotation (rotationZ);
            if (!isFacingRight && armScale.y > 0) {
                FlipArm (armScale);
            }
        }
    }
}
