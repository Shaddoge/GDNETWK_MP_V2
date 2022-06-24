using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public bool isReady = false;
    public Transform[] wheels = new Transform[4];

    public void Initialize(int _id, string _username)
    {
        this.id = _id;
        this.username = _username;
    }

    public void LerpPos(Vector3 _newPosition)
    {
        //Vector3.MoveTowards(transform.position, _newPosition, Time.fixedDeltaTime);
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

        Vector3 oldPos = transform.position;

        while(Time.time < oldTime + Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(oldPos, _newPosition, (Time.time - oldTime) / Time.fixedDeltaTime);
            yield return null;
        }
        transform.position = Vector3.LerpUnclamped(oldPos, _newPosition, 1.1f);
        //transform.position = _newPosition;
    }

    private IEnumerator LerpToNewRotation(Quaternion _newRotation)
    {
        float oldTime = Time.time;

        Quaternion oldRot = transform.rotation;

        while(Time.time < oldTime + Time.fixedDeltaTime)
        {
            transform.rotation = Quaternion.Lerp(oldRot, _newRotation, (Time.time - oldTime) / Time.fixedDeltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.LerpUnclamped(oldRot, _newRotation, 1.1f);
        //transform.rotation = _newRotation;
    }

    private IEnumerator LerpAllWheels(List<Vector3> _newPositions, List<Quaternion> _newRotations)
    {
        float oldTime = Time.time;

        Vector3[] oldPos = new Vector3[4];
        Quaternion[] oldRot = new Quaternion[4];

        for (int i = 0; i < wheels.Length; i++)
        {
            oldPos[i] = wheels[i].position;
            oldRot[i] = wheels[i].rotation;
        }

        while(Time.time < oldTime + Time.fixedDeltaTime)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].position = Vector3.Lerp(oldPos[i], _newPositions[i], (Time.time - oldTime) / Time.fixedDeltaTime);
                wheels[i].rotation = Quaternion.Lerp(oldRot[i], _newRotations[i], (Time.time - oldTime) / Time.fixedDeltaTime);
            }
            yield return null;
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].position = Vector3.LerpUnclamped(oldPos[i], _newPositions[i], 1.1f);
            wheels[i].rotation = Quaternion.LerpUnclamped(oldRot[i], _newRotations[i], 1.1f);

            wheels[i].position = _newPositions[i];
            wheels[i].rotation = _newRotations[i];
        }
    }
}
