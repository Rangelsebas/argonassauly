﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    float xThrow, yThrow;

    bool isControlEnable = true;

    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float speed = 20f;
    [Tooltip("In m")] [SerializeField] float xRange = 10f;
    [Tooltip("In m")] [SerializeField] float yRange = 8f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-position Based")]
    [SerializeField] float positionPitchFactor = -3.5f;
    [SerializeField] float positionYawFactor = 1f;

    [Header("Control-throw Based")]
    [SerializeField] float controlPitchFactor = -3f;
    [SerializeField] float controlRollFactor = -30f;

    // Update is called once per frame
    void Update()
    {
        if(isControlEnable) 
        {        
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    void OnPlayerDeath() 
    {
        isControlEnable = false;
    }
    
    private void ProcessRotation() 
    {
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitch = pitchDueToControlThrow + pitchDueToPosition;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation() 
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * speed * Time.deltaTime;
        float yOffset = yThrow * speed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float rawYPos = transform.localPosition.y + yOffset;

        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    private void ProcessFiring() 
    {
        if (Input.GetButton("Fire")) 
        {
            SetGunsActive(true);
        }
        else 
        {
            SetGunsActive(false);
        }
    }

    void SetGunsActive(bool isActive) 
    {
        foreach (GameObject gun in guns) 
        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
