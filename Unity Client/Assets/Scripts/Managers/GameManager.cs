using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    [Header("Objects")]
    [SerializeField] private GameObject[] tracks;

    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of GameManager instance");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = GameObject.Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = GameObject.Instantiate(playerPrefab, _position, _rotation);
        }

        AddProfile(_id, _username);

        PlayerManager spawnedPlayer = _player.GetComponent<PlayerManager>();
        spawnedPlayer.Initialize(_id, _username);
        players.Add(_id, spawnedPlayer);
    }

    public void AddProfile(int _id, string _username)
    {
        if (_id == Client.instance.myId)
        {
            ProfileManager.instance.CreateLocalPlayerProfile(_id, _username);
        }
        else
        {
            ProfileManager.instance.CreatePlayerProfile(_id, _username);
        }
    }

    public void ChangeTrack(int _trackId)
    {
        Debug.Log($"Track change! {_trackId}");
        for (int i = 0; i < tracks.Length; i++)
        {
            if(i == _trackId)
            {
                tracks[i].SetActive(true);
            }
            else
            {
                tracks[i].SetActive(false);
            }
        }
    }
}
