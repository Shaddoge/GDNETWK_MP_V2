using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = mainCamera.transform.rotation * originalRotation;
    }
}
