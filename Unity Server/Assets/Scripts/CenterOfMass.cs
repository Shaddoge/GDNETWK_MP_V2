using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    private Rigidbody rBody;
    [SerializeField] private Vector3 centerOfMass;
    [SerializeField] private bool Awake;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        if(rBody == null)
            Destroy(this);

    }

    // Update is called once per frame
    void Update()
    {
        rBody.centerOfMass = centerOfMass;
        rBody.WakeUp();
        Awake = !rBody.IsSleeping();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + transform.rotation * centerOfMass, new Vector3(0.2f, 0.2f, 0.2f));
    }
}
