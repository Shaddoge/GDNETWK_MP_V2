using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI position;
    [SerializeField] private PlayerPosition playerPosPrefab;

    private List<PlayerPosition> playerPositions = new List<PlayerPosition>();

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

    public void UpdatePositionList()
    {
        /*for (int i = 0; i < playerPositions.Count; i++)
        {
            playerPositions[i].id
        }*/
    }
}
