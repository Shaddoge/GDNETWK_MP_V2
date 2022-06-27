using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int placement;
    public bool isReady = false;
    public Transform[] wheels = new Transform[4];
    [SerializeField] private TextMeshProUGUI displayName;

    public void Initialize(int _id, string _username)
    {
        this.id = _id;
        this.username = _username;

        if (displayName != null)
        {
            displayName.text = username;
        }
    }

    public void LerpPos(Vector3 _newPosition)
    {
        transform.position = _newPosition;
        //StartCoroutine(LerpToNewPosition(_newPosition));
    }

    public void LerpRot(Quaternion _newRotation)
    {
        transform.rotation = _newRotation;
        //StartCoroutine(LerpToNewRotation(_newRotation));
    }

    public void LerpWheels(List<Vector3> _newPositions, List<Quaternion> _newRotation)
    {
        StartCoroutine(LerpAllWheels(_newPositions, _newRotation));
    }

    private IEnumerator LerpToNewPosition(Vector3 _newPosition)
    {
        float oldTime = Time.time;
        float currTime = 0f;
        Vector3 oldPos = transform.position;

        while(currTime < Time.fixedDeltaTime)
        {
            currTime += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPos, _newPosition, currTime / Time.fixedDeltaTime);
            //transform.position = Vector3.MoveTowards(oldPos, _newPosition, currTime / Time.fixedDeltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.position = Vector3.LerpUnclamped(oldPos, _newPosition, currTime / Time.fixedDeltaTime);
        //transform.position = _newPosition;
    }

    private IEnumerator LerpToNewRotation(Quaternion _newRotation)
    {
        float oldTime = Time.time;
        float currTime = 0f;
        Quaternion oldRot = transform.rotation;

        while(currTime < Time.fixedDeltaTime)
        {
            currTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(oldRot, _newRotation, currTime / Time.fixedDeltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.LerpUnclamped(oldRot, _newRotation, currTime / Time.fixedDeltaTime);
        //transform.rotation = _newRotation;
    }

    private IEnumerator LerpAllWheels(List<Vector3> _newPositions, List<Quaternion> _newRotations)
    {
        float oldTime = Time.time;
        float currTime = 0f;

        Vector3[] oldPos = new Vector3[4];
        Quaternion[] oldRot = new Quaternion[4];

        for (int i = 0; i < wheels.Length; i++)
        {
            oldPos[i] = wheels[i].position;
            oldRot[i] = wheels[i].rotation;
        }

        while(currTime < Time.fixedDeltaTime)
        {
            currTime += Time.deltaTime;
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].position = Vector3.Lerp(oldPos[i], _newPositions[i], currTime / Time.fixedDeltaTime);
                wheels[i].rotation = Quaternion.Lerp(oldRot[i], _newRotations[i], currTime / Time.fixedDeltaTime);
            }
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].position = Vector3.LerpUnclamped(oldPos[i], _newPositions[i], currTime / Time.fixedDeltaTime);
            wheels[i].rotation = Quaternion.LerpUnclamped(oldRot[i], _newRotations[i], currTime / Time.fixedDeltaTime);

            //wheels[i].position = _newPositions[i];
            //wheels[i].rotation = _newRotations[i];
        }
    }
}
