using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    // Acceleration
    private const float accelerationSpeed = 1f / Constants.TICKS_PER_SEC;
    private const float decelerationSpeed = 0.1f / Constants.TICKS_PER_SEC;
    private const float topSpeed = 10f / Constants.TICKS_PER_SEC;
    private float currentSpeed = 0f;

    // Steering
    private const float turnSpeed = 0.5f / Constants.TICKS_PER_SEC;
    private const float turnReduction = 0.1f / Constants.TICKS_PER_SEC;
    private const float maxTurnSpeed = 1.5f / Constants.TICKS_PER_SEC;
    private float currentTurnSpeed = 0f;

    private bool[] inputs;

    public void Initialize(int _id, string _username)
    {
        this.id = _id;
        this.username = _username;

        this.inputs = new bool[4];
    }

    public void FixedUpdate()
    {
        Vector2 _inputDirection = Vector2.zero;
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

        Steer(_inputDirection.x);
        Accelerate(_inputDirection.y);
    }

    private void Steer(float _direction)
    {
        Quaternion newRot = Quaternion.identity;
        if (currentTurnSpeed > 0)
        {
            currentTurnSpeed -= turnReduction;
            currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, 0, maxTurnSpeed);
        }
        else if (currentTurnSpeed < 0)
        {
            currentTurnSpeed += turnReduction;
            currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, -maxTurnSpeed, 0);
        }
        //Console.WriteLine(_direction);
        currentTurnSpeed += _direction * turnSpeed;
        currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, -maxTurnSpeed, maxTurnSpeed);
            
        newRot.y = currentTurnSpeed * (currentSpeed * 10);

        transform.rotation *= newRot;
        ServerSend.PlayerRotation(this);
    }

    private void Accelerate(float _direction)
    {
        //Vector3 _moveDirection = transform.right * _inputDirection.X + transform.forward * _inputDirection.Y;

        if (currentSpeed > 0)
        {
            currentSpeed -= decelerationSpeed;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, topSpeed);
        }
                
        else if (currentSpeed < 0)
        {
            currentSpeed += decelerationSpeed;
            currentSpeed = Mathf.Clamp(currentSpeed, -topSpeed, 0);
        }
                
        currentSpeed += _direction * accelerationSpeed;
        currentSpeed = Mathf.Clamp(currentSpeed, -topSpeed, topSpeed);

        //Console.WriteLine(currentSpeed);
        transform.position += transform.forward * currentSpeed;

        ServerSend.PlayerPosition(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        this.inputs = _inputs;
        transform.rotation = _rotation;
    }
}
