using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    [SerializeField] private int maxPlayers = 4;
    [SerializeField] private int port = 8888;

    [Header("Spawn Point")]
    [SerializeField] private Transform[] spawnPoints;

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
            Debug.Log("Removing the copy of client instance");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 128;


        Server.Start(maxPlayers, port);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer(int id)
    {
        Debug.Log(id);

        return Instantiate(playerPrefab, spawnPoints[id-1].position, Quaternion.identity).GetComponent<Player>();
    }
}
