using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    [SerializeField] private int maxPlayers = 4;
    [SerializeField] private int port = 7777;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of NetworkManager instance");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 64;

        Server.Start(maxPlayers, port);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer(int _playerId)
    {
        Vector3 spawnPos = GameManager.instance.GetCurrentSpawnPos(_playerId);
        Quaternion spawnRot = GameManager.instance.GetCurrentSpawnRot(_playerId);
        return Instantiate(playerPrefab, spawnPos, spawnRot).GetComponent<Player>();
    }
}
