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
        public float angle;

        // private Vector3 direction; 
        private float moveSpeed = 5f / Constants.TICS_PER_SEC;
        private float rotSpeed = 50f / Constants.TICS_PER_SEC;
        private bool[] inputs;

        public Player (int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;

            angle = 0.0f;

            inputs = new bool[4];
        }
        public void Update()
        {
            Vector3 inputDirection = Vector3.Zero;
            float heading = angle + 90f;
            float inputAngle = 0.0f;
            // W
            if (inputs[0] == true)
            {
                inputDirection += new Vector3(MathF.Cos(heading * Constants.DEG_2_RAD), MathF.Sin(heading * Constants.DEG_2_RAD), 0.0f);
            }
            // S
            if (inputs[1] == true)
            {
            }
            // A
            if (inputs[2] == true)
            {
                angle += 5.0f;
            }
            // D
            if (inputs[3] == true)
            {
                angle -= 5.0f;
            }

            Move(inputDirection, inputAngle);
        }

        private void Move(Vector3 inputDirection, float inputAngle)
        {
            //Vector3 forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
            //Vector3 right = Vector3.Normalize(Vector3.Cross(forward, new Vector3(0, 1, 0)));

            //Vector3 moveDirection = right * inputDirection.X + forward * inputDirection.Y;
            position += inputDirection * moveSpeed;
            angle += inputAngle * rotSpeed;

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
 
        public void SetInputs(bool[] _inputs, float _rotation)
        {
            inputs = _inputs;
            angle = _rotation;
        }
    }
}
