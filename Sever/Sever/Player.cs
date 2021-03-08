using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Sever
{
    class Player
    {
        public int id;
        public string username;
        public Vector3 position;
        public Vector3 velocity;
        public float rotation;
        public float heading;

        private float moveSpeed = 0.1f / Constants.TICKS_PER_SEC;
        private bool[] inputs;
        public Player(int _id, string _username, Vector3 spawnPosition)
        {
            id = _id;
            username = _username;
            position = spawnPosition;
            rotation = 0.0f;
            heading = 0.0f;
            velocity = Vector3.Zero;
            inputs = new bool[4];
        }

        public void Update()
        {
            if (inputs[0] == true)
            {
                velocity.X += MathF.Cos(Constants.Deg2Rad * rotation + 90.0f);
                velocity.Y += MathF.Sin(Constants.Deg2Rad * rotation + 90.0f);
            }
            if (inputs[2] == true)
            {
                rotation += 5f;     
            }
            if (inputs[3] == true)
            {
                rotation -= 5f;
            }

            Move();
        }

        private void Move ()
        {
            position += velocity * moveSpeed; 
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
        
        public void SetInputs (bool[] _inputs, float _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }
    }
}
