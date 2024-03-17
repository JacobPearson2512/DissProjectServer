using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ProjectServer
{
    // TEST COMMIT
    class Player
    {
        public int id;
        public string username;

        public int maxHP;
        public int currentHP;
        public int numberPotions;
        public bool isBlocking;
        public string currentMove;
        public float defense;
        public int timesHit;

        public Player(int _id, string _username, int _maxHP,  int _numberPotions)
        {
            id = _id;
            username = _username;
            maxHP = _maxHP;
            currentHP = _maxHP;
            numberPotions = _numberPotions;
            isBlocking = false;
            defense = 1.0f;
            currentMove = "";
            timesHit = 0;
        }
    }
}
