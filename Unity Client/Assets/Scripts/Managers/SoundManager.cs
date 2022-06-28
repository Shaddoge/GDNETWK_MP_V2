using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Music Source")]
    public AudioSource MusicSource;
    [Header("Car Source")]
    public AudioSource CarSFXSource;
    [Header("Button Source")]
    public AudioSource ButtonSource;

    [Header("Sound List")]
    public AudioClip BGM;
    public AudioClip carRevSFX;
    public AudioClip carDriveSFX;
    public AudioClip carBrakeSFX;
    public AudioClip carIdleSFX;

    public AudioClip buttonSFX;

    private bool canTriggerBrake = true;
    public bool inCountDownCarSFX = false;



    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of ProfileManager instance");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonSFX()
    {
        ButtonSource.PlayOneShot(buttonSFX);
    }

    public void PlayBGM()
    {
        MusicSource.clip = BGM;
        MusicSource.Play();
    }

    public void PlayCarRev()
    {
        // play when car speeds up
        if (CarSFXSource.clip == carRevSFX && CarSFXSource.isPlaying)
        {
            return;
        }
        else
        {
            Debug.Log("play rev");
            CarSFXSource.Stop();
            CarSFXSource.clip = carRevSFX;
            CarSFXSource.Play();
            canTriggerBrake = true;

        }
    }

    public void PlayCarDrive()
    {
        // dont play when is already playing
        if (CarSFXSource.clip == carDriveSFX && CarSFXSource.isPlaying)
        {
            return;
        }
        else
        {
            Debug.Log("play drive");
            CarSFXSource.Stop();
            CarSFXSource.clip = carDriveSFX;
            CarSFXSource.Play();
            canTriggerBrake = true;

        }
    }
    public void PlayCarIdle()
    {
        if (CarSFXSource.clip == carIdleSFX && CarSFXSource.isPlaying)
        {
            return;
        }
        else
        {
            CarSFXSource.Stop();
            Debug.Log("play Idle");
            CarSFXSource.clip = carIdleSFX;
            CarSFXSource.Play();
            canTriggerBrake = true;

        }
    }

    public void PlayCarBrake()
    {
        if (CarSFXSource.clip == carBrakeSFX && CarSFXSource.isPlaying)
        {
            return;
        }

        if (canTriggerBrake == true)
        {
            Debug.Log(CarSFXSource.clip.name);
            // play when car is max speed
            CarSFXSource.PlayOneShot(carBrakeSFX);
            canTriggerBrake = false;
        }
        
    }
}
