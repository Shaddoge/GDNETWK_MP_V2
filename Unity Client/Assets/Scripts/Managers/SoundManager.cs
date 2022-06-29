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

    [Header("Track List")]
    [SerializeField] private List <AudioClip> TrackList;

    [Header("Car SFX")]
    [SerializeField] private AudioClip carstartSFX;
    [SerializeField] private AudioClip carRevSFX;
    [SerializeField] private AudioClip carDriveSFX;
    [SerializeField] private AudioClip carBrakeSFX;
    [SerializeField] private AudioClip carIdleSFX;

    [Header("Button SFX")]
    [SerializeField] private AudioClip buttonSFX;


    private bool canTriggerBrake = true;
    public bool inCountDownCarSFX = false;

    public bool inLobby = true;

    [SerializeField] int maxTrackIndex=0;
    [SerializeField] int currTrackIndex = 0;

    [SerializeField] bool flag = false;







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
        maxTrackIndex = TrackList.Count;

        //// play the first track in the list
        MusicSource.clip = TrackList[currTrackIndex];
        MusicSource.Play();
        CarSFXSource.loop = false;


        // play the car start sfx
        CarSFXSource.Stop();
        CarSFXSource.clip = carstartSFX;
        CarSFXSource.Play();
        CarSFXSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inLobby == true)
        {
            if(CarSFXSource.isPlaying == false)
            {
                Debug.Log("Append Track");
                CarSFXSource.clip = carIdleSFX; // transition to idle sfx 
                CarSFXSource.Play();
                CarSFXSource.loop = true;
            }
        }

        CheckRadioInput(); // for player input
        QueueRadio();// auto que

    }

    public void PlayButtonSFX()
    {
        ButtonSource.PlayOneShot(buttonSFX);
    }

    public void PlayBGM()
    {
        
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

    public void QueueRadio()
    {
        // check if track is stil playing
        if (MusicSource.isPlaying == false)
        {
            currTrackIndex++; // next track in list
            PlayTrack();// play track based on index

        }
    }

    private void CheckRadioInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && flag == true)
        {
            currTrackIndex--;
            flag = false;
            PlayTrack();// play track based on index
        }
        else if (Input.GetKeyDown(KeyCode.E) && flag == true)
        {
            currTrackIndex++;
            flag = false;
            PlayTrack();// play track based on index
        }
        else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            flag = true;
        }

        

    }

    public void PlayTrack()
    {
        if (currTrackIndex > maxTrackIndex)
        {
            currTrackIndex = 0;
        }
        else if (currTrackIndex <0)
        {
            currTrackIndex = maxTrackIndex-1;
        }
        // next track

        MusicSource.clip = TrackList[currTrackIndex];
        Debug.Log("Now Playing: " + TrackList[currTrackIndex].name);
        MusicSource.Play();
    }
}
