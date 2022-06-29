using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSFX : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Car SFX")]
    [SerializeField] private AudioClip carStartSFX;
    [SerializeField] private AudioClip carStartDriveSFX;
    [SerializeField] private AudioClip carDriveSFX;
    [SerializeField] private AudioClip carBrakeSFX;
    [SerializeField] private AudioClip carSkidSFX;
    [SerializeField] private AudioClip carIdleSFX;

    private bool canTriggerBrake = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            Debug.Log("Append Track");
            audioSource.clip = carIdleSFX; // transition to idle sfx 
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void Stop()
    {
        if (audioSource.clip == carIdleSFX || audioSource.clip == carStartSFX) return;

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

    public void PlayCarIdle()
    {
        if (audioSource.clip == carIdleSFX && audioSource.isPlaying)
        {
            return;
        }

        audioSource.Stop();
        Debug.Log("play Idle");
        audioSource.clip = carIdleSFX;
        audioSource.Play();
        canTriggerBrake = true;
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
        if (audioSource.clip == carSkidSFX && audioSource.isPlaying)
        {
            return;
        }

        audioSource.PlayOneShot(carSkidSFX);
    }
}
