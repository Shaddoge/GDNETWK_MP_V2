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

        private const float accelerationSpeed = 1f / Constants.TICKS_PER_SEC;
        private const float maxSpeed = 10f / Constants.TICKS_PER_SEC;
        private const float decelerationSpeed = 0.1f / Constants.TICKS_PER_SEC;
        private float currentSpeed = 0f;

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

            Move(_inputDirection);
        }

        private void Move(Vector2 _inputDirection)
        {
            Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
            Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));

            Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;

            if(currentSpeed > 0)
            {
                currentSpeed -= decelerationSpeed;
                currentSpeed = Math.Clamp(currentSpeed, 0, maxSpeed);
            }
                
            else if(currentSpeed < 0)
            {
                currentSpeed += decelerationSpeed;
                currentSpeed = Math.Clamp(currentSpeed, -maxSpeed, 0);
            }
                
            currentSpeed += _inputDirection.Y * accelerationSpeed;
            currentSpeed = Math.Clamp(currentSpeed, -maxSpeed, maxSpeed);

            Console.WriteLine(currentSpeed);
            position += _forward * currentSpeed;

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }

        public void SetInput(bool[] _inputs, Quaternion _rotation)
        {
            this.inputs = _inputs;
            this.rotation = _rotation;
        }
    }
}
