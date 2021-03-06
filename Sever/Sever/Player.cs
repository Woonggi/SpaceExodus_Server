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
        public Vector3 rotation;
        public Player(int _id, string _username, Vector3 spawnPosition)
        {
            id = _id;
            username = _username;
            position = spawnPosition;
            rotation = new Vector3();
        }

    }
}
