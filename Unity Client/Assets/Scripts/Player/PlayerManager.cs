using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public bool isReady = false;
    public Transform[] wheels = new Transform[4];

    public void LerpPos(Vector3 _newPosition)
    {
        StartCoroutine(LerpToNewPosition(_newPosition));
    }

    public void LerpRot(Quaternion _newRotation)
    {
        StartCoroutine(LerpToNewRotation(_newRotation));
    }

    public void LerpWheels(List<Vector3> _newPositions, List<Quaternion> _newRotation)
    {
        StartCoroutine(LerpAllWheels(_newPositions, _newRotation));
    }

    private IEnumerator LerpToNewPosition(Vector3 _newPosition)
    {
        float oldTime = Time.time;
        //float currTime = 0f;

        while(Time.time < oldTime + Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, _newPosition, (Time.time - oldTime) / Time.fixedDeltaTime);
            yield return null;
        }
        transform.position = _newPosition;
    }

    private IEnumerator LerpToNewRotation(Quaternion _newRotation)
    {
        float oldTime = Time.time;
        //float currTime = 0f;

        while(Time.time < oldTime + Time.fixedDeltaTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, (Time.time - oldTime) / Time.fixedDeltaTime);
            //currTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = _newRotation;
    }

    private IEnumerator LerpAllWheels(List<Vector3> _newPositions, List<Quaternion> _newRotations)
    {
        float oldTime = Time.time;
        while(Time.time < oldTime + Time.fixedDeltaTime)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].position = Vector3.Lerp(wheels[i].position, _newPositions[i], (Time.time - oldTime) / Time.fixedDeltaTime);
                wheels[i].rotation = Quaternion.Lerp(wheels[i].rotation, _newRotations[i], (Time.time - oldTime) / Time.fixedDeltaTime);
            }
            //currTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].position = wheels[i].position;
            wheels[i].rotation = wheels[i].rotation;
        }
    }
}
