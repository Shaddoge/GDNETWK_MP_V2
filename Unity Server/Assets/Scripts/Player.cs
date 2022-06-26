using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public Checkpoint nextCheckpoint;
    public int placement = 1;
    public bool canMove = false;
    public bool isFinished = false;

    // Input
    private bool[] inputs;
    
    // Current Values
    private bool isBreaking;
    private float currentBreakForce = 0f;
    private float currentSteerAngle = 0f;
    
    // Motor force
    private const float motorForce = 2000f;
    private const float breakForce = 4000f;
    private const float maxSteerAngle = 30f;

    public WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheels = new Transform[4];

    // Lobby
    public bool isReady = false;

    public void Initialize(int _id, string _username)
    {
        this.id = _id;
        this.username = _username;

        this.placement = 1;
        this.isFinished = false;
        this.isReady = false;
        this.inputs = new bool[5];
        this.nextCheckpoint = CheckpointHandler.instance.GetFirstCheckpoint();
    }

    public void ResetValues()
    {
        this.placement = 1;
        this.isFinished = false;
        this.isReady = false;
    }

    public void FixedUpdate()
    {
        Vector2 _inputDirection = Vector2.zero;
        isBreaking = false;

        if(inputs[0])
        {
            _inputDirection.y += 1;
        }
        if(inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if(inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if(inputs[3])
        {
            _inputDirection.x += 1;
        }
        if(inputs[4])
        {
            isBreaking = true;
        }

        HandleSteering(_inputDirection.x);
        HandleMotor(_inputDirection.y);
        UpdateWheels();

        ServerSend.PlayerRotation(this);
        ServerSend.PlayerPosition(this);
        ServerSend.PlayerWheels(this);
        ServerSend.PlayerState(this);
    }

    private void HandleMotor(float _direction)
    {
        // Front Wheels
        wheelColliders[0].motorTorque = _direction * motorForce;
        wheelColliders[1].motorTorque = _direction * motorForce;
        // Rear Wheels
        //wheelColliders[2].motorTorque = _direction * motorForce;
        //wheelColliders[3].motorTorque = _direction * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        
        ApplyBreak();
    }

    private void ApplyBreak()
    {
        foreach(WheelCollider collider in wheelColliders)
        {
            collider.brakeTorque = currentBreakForce;
        }
    }

    private void HandleSteering(float _direction)
    {
        currentSteerAngle = maxSteerAngle * _direction;
        wheelColliders[0].steerAngle = currentSteerAngle;
        wheelColliders[1].steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            Vector3 pos;
            Quaternion rot;

            wheelColliders[i].GetWorldPose(out pos, out rot);
            wheels[i].position = pos;
            wheels[i].rotation = rot;
        }
    }

    public void SetInput(bool[] _inputs)
    {
        if (!this.canMove) return;
        this.inputs = _inputs;
    }

    public void SetReady(bool isReady)
    {
        this.isReady = isReady;
    }
}
