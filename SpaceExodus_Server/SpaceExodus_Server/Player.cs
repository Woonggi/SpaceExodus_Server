using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace SpaceExodus_Server
{
    class Player
    {
        public int id;
        public string username;

        public Vector3 position;
        public Quaternion rotation;

        private float moveSpeed = 5f / Constants.TICS_PER_SEC;
        private bool[] inputs;

        public Player (int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;

            inputs = new bool[4];
        }
        public void Update()
        {
            Vector2 inputDirection = Vector2.Zero;
            // W
            if (inputs[0] == true)
            {
                inputDirection.Y += 1f;
            }
            // S
            if (inputs[1] == true)
            {
                inputDirection.Y -= 1f;
            }
            // A
            if (inputs[2] == true)
            {
                inputDirection.X += 1f;
            }
            // D
            if (inputs[3] == true)
            {
                inputDirection.X -= 1f;
            }

            Move(inputDirection);
        }

        private void Move(Vector2 inputDirection)
        {
            Vector3 forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
            Vector3 right = Vector3.Normalize(Vector3.Cross(forward, new Vector3(0, 1, 0)));

            Vector3 moveDirection = right * inputDirection.X + forward * inputDirection.Y;
            position += moveDirection * moveSpeed;

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
 
        public void SetInputs(bool[] _inputs, Quaternion _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }
    }
}
