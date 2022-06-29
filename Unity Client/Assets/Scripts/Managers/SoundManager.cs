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
    [SerializeField] private AudioClip carRevSFX;
    
    [SerializeField] private AudioClip radioSFX;

    [Header("Button SFX")]
    [SerializeField] private AudioClip buttonSFX;

    public bool inCountDownCarSFX = false;

    [SerializeField] int maxTrackIndex = 0;
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
            Debug.Log("Removing the copy of SoundManager instance");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        maxTrackIndex = TrackList.Count - 1;

        //// play the first track in the list
        MusicSource.clip = TrackList[currTrackIndex];
        MusicSource.Play();
        CarSFXSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRadioInput(); // for player input
        QueueRadio();// auto queue
    }

    public void PlayButtonSFX()
    {
        ButtonSource.PlayOneShot(buttonSFX);
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

        }
    }

    private void QueueRadio()
    {
        if (!MusicSource.isPlaying)
        {
            currTrackIndex++;
            PlayTrack();
        }
    }

    private void CheckRadioInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && flag == true)
        {
            currTrackIndex--;
            flag = false;
            MusicSource.clip = radioSFX;
            MusicSource.Play();
        }
        else if (Input.GetKeyDown(KeyCode.E) && flag == true)
        {
            currTrackIndex++;
            flag = false;
            MusicSource.clip = radioSFX;
            MusicSource.Play();
        }
        else if (Input.GetKeyDown(KeyCode.M) && flag == true)
        {
            flag = false;
            MusicSource.mute = !MusicSource.mute;
            if (MusicSource.mute)
            {
                Debug.Log("Radio Off");
            }
            else
            {
                Debug.Log("Radio On");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Comma) && flag == true)
        {
            flag = false;
            float musicVol = MusicSource.volume;
            musicVol -= 0.01f;
            AdjustMusicVolume(musicVol);
        }
        else if (Input.GetKeyDown(KeyCode.Period) && flag == true)
        {
            flag = false;
            float musicVol = MusicSource.volume;
            musicVol += 0.01f;
            AdjustMusicVolume(musicVol);
        }

        else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.Comma) || Input.GetKeyUp(KeyCode.Period))
        {
            flag = true;
        }

        if (MusicSource.clip == radioSFX && !MusicSource.isPlaying)
        {
            PlayTrack();
        }
    }

    public void PlayTrack()
    {
        if (currTrackIndex > maxTrackIndex)
        {
            currTrackIndex = 0;
        }
        else if (currTrackIndex < 0)
        {
            currTrackIndex = maxTrackIndex - 1;
        }
        // next track

        MusicSource.clip = TrackList[currTrackIndex];
        Debug.Log("Now Playing: " + TrackList[currTrackIndex].name);
        MusicSource.Play();
    }

    private void AdjustMusicVolume(float newVolume)
    {
        newVolume = Mathf.Clamp(newVolume, 0, 1);

        MusicSource.volume = newVolume;

        Debug.Log($"Radio Volume: {newVolume * 100}");
    }
}
