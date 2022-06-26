using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInputField : MonoBehaviour
{
    [SerializeField] private InputField[] inputFields;

    private int nSelected = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && (Input.GetKey(KeyCode.LeftShift)))
        {
            bool selected = false;
            for (int i = 0; i < inputFields.Length; i++)
            {
                if (inputFields[i].isFocused)
                {
                    nSelected = i;
                    selected = true;
                    break;
                }
            }
            if (!selected) return;

            nSelected = (nSelected - 1) % inputFields.Length;

            inputFields[nSelected].ActivateInputField();
        }

        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool selected = false;
            for (int i = 0; i < inputFields.Length; i++)
            {
                if (inputFields[i].isFocused)
                {
                    nSelected = i;
                    selected = true;
                    break;
                }
            }
            if (!selected) return;

            nSelected = (nSelected + 1) % inputFields.Length;

            inputFields[nSelected].ActivateInputField();
        }
    }
}
