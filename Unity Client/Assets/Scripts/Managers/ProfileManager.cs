using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerProfile
{
    public int id;
    public GameObject panel;
}

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    public static List<PlayerProfile> PlayerProfileList = new List<PlayerProfile>();

    [Header("Prefabs")] 
    
    [SerializeField] private GameObject profileList;
    [SerializeField] private GameObject localProfilePrefab;
    [SerializeField] private GameObject profilePrefab;

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

    public void Update()
    {
        /*for (int i = 0; i < PlayerProfileList.Count; i++)
        {
            if (PlayerProfileList[i].id != Client.instance.myId)
            {
                if (GameManager.players[i + 1].isReady)
                {
                    PlayerProfileList[i].panel.transform.GetChild(1).gameObject.SetActive(true);
                }

                else
                {
                    PlayerProfileList[i].panel.transform.GetChild(1).gameObject.SetActive(false);
                }
            }

            // Should be called when informed from the server
            if (GameManager.instance.startGame)
            {
                if (PlayerProfileList[i].id == Client.instance.myId)
                {
                    PlayerProfileList[i].panel.transform.GetChild(1).GetComponent<Toggle>().interactable = false;
                }
            }
        }*/
    }

    public void TimerStarted()
    {
        PlayerProfileList[Client.instance.myId - 1].panel.transform.GetChild(1).GetComponent<Toggle>().interactable = false;
    }

    public void ToggleCheck(int _id, bool _isReady)
    {
        PlayerProfileList[_id - 1].panel.transform.GetChild(1).gameObject.SetActive(_isReady);
    }

    public void CreateLocalPlayerProfile(int _id, string _username)
    {
        PlayerProfile _profile;

        GameObject _profilePanel = GameObject.Instantiate(localProfilePrefab, profileList.transform);
        _profilePanel.transform.GetChild(0).GetComponent<Text>().text = _username;
        _profile.id = _id;
        _profile.panel = _profilePanel;

        PlayerProfileList.Add(_profile);
    }

    public void CreatePlayerProfile(int _id, string _username)
    {
        PlayerProfile _profile;

        GameObject _profilePanel = GameObject.Instantiate(profilePrefab, profileList.transform);
        _profilePanel.transform.GetChild(0).GetComponent<Text>().text = _username;
        _profile.id = _id;
        _profile.panel = _profilePanel;

        PlayerProfileList.Add(_profile);
    }

    public void RemoveProfile(int _id)
    {
        for (int i = 0; i < PlayerProfileList.Count; i++)
        {
            if (PlayerProfileList[i].id != Client.instance.myId)
            {
                Destroy(PlayerProfileList[i].panel);
                PlayerProfileList.RemoveAt(i);
            }
        }
    }
}
