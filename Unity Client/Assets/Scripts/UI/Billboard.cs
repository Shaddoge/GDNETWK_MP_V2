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
        mainCamera = Camera.current;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (mainCamera != Camera.current)
            mainCamera = Camera.current;

        if (mainCamera == null) return;
        transform.LookAt(2 * transform.position - mainCamera.transform.position);

        //transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        //transform.rotation = mainCamera.transform.rotation * originalRotation;
    }
}
