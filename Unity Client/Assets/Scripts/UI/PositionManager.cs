using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionManager : MonoBehaviour
{
    public static PositionManager instance;
    [SerializeField] private TextMeshProUGUI position;

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

    public void SetPosition(int _place)
    {
        position.text = _place.ToString();
        switch(_place)
        {
            case 1: position.text += "st"; break;
            case 2: position.text += "nd"; break;
            case 3: position.text += "rd"; break;
            case 4: position.text += "th"; break;
            default: position.text += "th"; break;
        }
    }
}
