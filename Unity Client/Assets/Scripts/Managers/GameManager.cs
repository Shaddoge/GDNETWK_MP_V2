using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>(); 

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

        PlayerManager spawnedPlayer = _player.GetComponent<PlayerManager>();
        spawnedPlayer.id = _id;
        spawnedPlayer.username = _username;
        players.Add(_id, spawnedPlayer);
    }
}
