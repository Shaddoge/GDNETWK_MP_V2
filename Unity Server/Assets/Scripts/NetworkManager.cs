using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    [SerializeField] private int maxPlayers = 4;
    [SerializeField] private int port = 8888;

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
        Application.targetFrameRate = 64;

        #if UNITY_EDITOR
        Debug.Log("Build project to make the server work");
        #else
        Server.Start(maxPlayers, port);
        #endif
    }

    public Player InstantiatePlayer()
    {
        Debug.Log("Instantiating");
        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
    }
}
