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
        public bool hasWon;

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
            hasWon = false;
        }

        // this constructor is used for constructing a duplicate player for corruption purposes.
        public Player(int _id, string _username, int _maxHP, int _numberPotions, bool _hasWon, int _currentHP, float _defense, int _timesHit, string _currentMove) 
        {
            id = _id;
            username = _username;
            maxHP = _maxHP;
            currentHP = _currentHP;
            numberPotions = _numberPotions;
            isBlocking = false;
            defense = _defense;
            currentMove = _currentMove;
            timesHit = _timesHit;
            hasWon = _hasWon;
        }
    }
}
