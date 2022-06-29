using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource oneShotSource;

    [Header("Car SFX")]
    [SerializeField] private AudioClip carStartSFX;
    [SerializeField] private AudioClip carStartDriveSFX;
    [SerializeField] private AudioClip carDriveSFX;
    [SerializeField] private AudioClip carBrakeSFX;
    [SerializeField] private AudioClip carSkidSFX;
    [SerializeField] private AudioClip carIdleSFX;

    private bool canTriggerBrake = false;
    private float idleCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        oneShotSource.PlayOneShot(carStartSFX);
    }

    private void Update()
    {
        idleCounter += Time.deltaTime;

        if (idleCounter > 0.1f)
        {
            if(!audioSource.isPlaying)
                oneShotSource.PlayOneShot(carIdleSFX, Random.Range(0.5f, 1f));

            idleCounter = 0f;
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void PlayCarDrive()
    {
        // dont play when is already playing
        if ((audioSource.clip == carDriveSFX || audioSource.clip == carStartDriveSFX) && audioSource.isPlaying)
        {
            return;
        }
        
        if (audioSource.clip == carStartDriveSFX && !audioSource.isPlaying)
        {
            Debug.Log("play drive");
            audioSource.Stop();
            audioSource.clip = carDriveSFX;
            audioSource.loop = true;
            audioSource.Play();
            canTriggerBrake = true;
        }
        else
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.clip = carStartDriveSFX;
            audioSource.Play();
            canTriggerBrake = true;
        }
    }

    public void PlayCarBrake()
    {
        if (audioSource.clip == carBrakeSFX && audioSource.isPlaying)
        {
            return;
        }

        if (canTriggerBrake)
        {
            Debug.Log(audioSource.clip.name);
            // play when car is max speed
            audioSource.PlayOneShot(carBrakeSFX);
            canTriggerBrake = false;
        }
    }

    public void PlayCarSkid()
    {
        oneShotSource.PlayOneShot(carSkidSFX);
    }
}
