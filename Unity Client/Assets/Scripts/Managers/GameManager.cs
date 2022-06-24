using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public bool startGame = false;
    private int numOfReady = 0;

    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;

    public void FixedUpdate()
    {
        foreach (var player in players)
        {
            if (player.Value.isReady) numOfReady++;
        }

        if (players.Count != 0 && players.Count == numOfReady)
        {
            startGame = true;
        }

        else
        {
            startGame = false;
        }

        numOfReady = 0;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of client instance");
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
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            ProfileManager.instance.CreateLocalPlayerProfile(_id, _username);
        }
        else
        {
            ProfileManager.instance.CreatePlayerProfile(_id, _username);
        }
    }
}
