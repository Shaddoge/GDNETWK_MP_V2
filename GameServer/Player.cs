using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;

        public Vector3 position;
        public Quaternion rotation;

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

        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            this.id = _id;
            this.username = _username;
            this.position = _spawnPosition;
            this.rotation = Quaternion.Identity;

            this.inputs = new bool[4];
        }

        public void Update()
        {
            Vector2 _inputDirection = Vector2.Zero;
            if(inputs[0])
            {
                _inputDirection.Y += 1;
            }
            if(inputs[1])
            {
                _inputDirection.Y -= 1;
            }
            if(inputs[2])
            {
                _inputDirection.X += 1;
            }
            if(inputs[3])
            {
                _inputDirection.X -= 1;
            }

            Steer(_inputDirection.X);
            Accelerate(_inputDirection.Y);
        }

        private void Steer(float _direction)
        {
            Quaternion newRot = Quaternion.Identity;
            if (currentTurnSpeed > 0)
            {
                currentTurnSpeed -= turnReduction;
                currentTurnSpeed = Math.Clamp(currentTurnSpeed, 0, maxTurnSpeed);
            }
            else if (currentTurnSpeed < 0)
            {
                currentTurnSpeed += turnReduction;
                currentTurnSpeed = Math.Clamp(currentTurnSpeed, -maxTurnSpeed, 0);
            }
            //Console.WriteLine(_direction);
            currentTurnSpeed += _direction * turnSpeed;
            currentTurnSpeed = Math.Clamp(currentTurnSpeed, -maxTurnSpeed, maxTurnSpeed);
            
            newRot.Y = -currentTurnSpeed * (currentSpeed * 10);

            rotation *= newRot;
            ServerSend.PlayerRotation(this);
        }

        private void Accelerate(float _direction)
        {
            Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
            //Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));
            //Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;

            if (currentSpeed > 0)
            {
                currentSpeed -= decelerationSpeed;
                currentSpeed = Math.Clamp(currentSpeed, 0, topSpeed);
            }
                
            else if (currentSpeed < 0)
            {
                currentSpeed += decelerationSpeed;
                currentSpeed = Math.Clamp(currentSpeed, -topSpeed, 0);
            }
                
            currentSpeed += _direction * accelerationSpeed;
            currentSpeed = Math.Clamp(currentSpeed, -topSpeed, topSpeed);

            //Console.WriteLine(currentSpeed);
            position += _forward * currentSpeed;

            ServerSend.PlayerPosition(this);
        }

        public void SetInput(bool[] _inputs, Quaternion _rotation)
        {
            this.inputs = _inputs;
            this.rotation = _rotation;
        }
    }
}
