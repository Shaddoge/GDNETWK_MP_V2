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

        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            this.id = _id;
            this.username = _username;
            this.position = _spawnPosition;
            this.rotation = Quaternion.Identity;
        }
    }
}
