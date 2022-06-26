using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public GameObject cameraConstraint;
    [SerializeField] float speed = 1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (cameraConstraint == null)
        {
            cameraConstraint = player.transform.Find("Camera Constraint").gameObject;
        }

        if (player != null && cameraConstraint != null)
        {

            Follow();
        }
    }

    private void Follow()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position,cameraConstraint.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(player.gameObject.transform.position);
    }
}
