using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ProjectServer
{
    class GameLogic
    {
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }

        // TODO: fine for now. Needs updating with defense logic, and potentially shrinking down for efficiency + tidiness.
        public static void HandleAction()
        {
            Queue<(int, string)> _moveQueue = ServerHandle.moveQueue;
            if (_moveQueue != null)
            {
                (int _player1ID, string _player1Move) = _moveQueue.Dequeue();
                (int _player2ID, string _player2Move) = _moveQueue.Dequeue();
                Player _player1 = Server.clients[_player1ID].player;
                Player _player2 = Server.clients[_player2ID].player;
                if (_player1Move == "Protect")
                {
                    _player1.isBlocking = true;
                    Console.WriteLine($"{_player1.username} used Protect.");
                }
                if (_player2Move == "Protect")
                {
                    _player2.isBlocking = true;
                    Console.WriteLine($"{_player2.username} used Protect.");
                }
                switch (_player1Move)
                {
                    case "Slash": 
                        Console.WriteLine($"{_player1.username} used Slash on {_player2.username}!");
                        if (!_player2.isBlocking)
                        {
                            _player2.currentHP -= 20;
                            Console.WriteLine($"HP left: {_player2.currentHP}");
                        }
                        else
                        {
                            Console.WriteLine("...But they blocked it!");
                        }
                        break;
                    case "Protect":
                        break;
                    case "Whirlwind":
                        Console.WriteLine($"{_player1.username} used Whirlwind Blade on {_player2.username}!");
                        if (!_player2.isBlocking)
                        {
                            _player2.currentHP -= 15;
                            _player2.defense = (float)Math.Round(_player2.defense * 8f) / 10;
                            Console.WriteLine($"{_player2.username}'s defense was lowered by 10%!");
                            Console.WriteLine($"HP left: {_player2.currentHP}");
                        }
                        else
                        {
                            Console.WriteLine("...But they blocked it!");
                        }
                        break;
                    case "Heal":
                        Console.WriteLine("Healed");
                        if (_player1.numberPotions > 0)
                        {
                            _player1.numberPotions -= 1;
                            _player1.currentHP += 50;
                            if (_player2.currentHP > 150)
                            {
                                _player2.currentHP = 150;
                            }
                        }
                        break;
                    case "Flurry":
                        Console.WriteLine("Flurry");
                        Random random = new Random();
                        Console.WriteLine($"{_player1.username} used Flurry on {_player2.username}!");
                        if (!_player2.isBlocking)
                        {
                            int _timesHit = random.Next(2, 6);
                            Console.WriteLine($"Hit {_timesHit} times!");
                            _player2.currentHP -= (_timesHit * 15);
                            Console.WriteLine($"HP left: {_player2.currentHP}");
                        }
                        else
                        {
                            Console.WriteLine("...But they blocked it!");
                        }
                        
                        break;
                }

                switch (_player2Move)
                {
                    case "Slash":
                        Console.WriteLine($"{_player2.username} used Slash on {_player1.username}!");
                        if (!_player1.isBlocking)
                        {
                            _player1.currentHP -= 20;
                            Console.WriteLine($"HP left: {_player1.currentHP}");
                        }
                        else
                        {
                            Console.WriteLine("...But they blocked it!");
                        }
                        break;
                    case "Protect":
                        break;
                    case "Whirlwind":
                        Console.WriteLine($"{_player2.username} used Whirlwind Blade on {_player1.username}!");
                        if (!_player1.isBlocking)
                        {
                            _player1.currentHP -= 15;
                            _player1.defense = (float)Math.Round(_player1.defense * 8f) / 10;
                            Console.WriteLine($"{_player1.username}'s defense was lowered by 10%!");
                            Console.WriteLine($"HP left: {_player1.currentHP}");
                        }
                        else
                        {
                            Console.WriteLine("...But they blocked it!");
                        }
                        break;
                    case "Heal":
                        Console.WriteLine("Healed");
                        if (_player2.numberPotions > 0)
                        {
                            _player2.numberPotions -= 1;
                            _player2.currentHP += 50;
                            if (_player2.currentHP > 150)
                            {
                                _player2.currentHP = 150;
                            }
                        }
                        break;
                    case "Flurry":
                        Console.WriteLine("Flurry");
                        Random random = new Random();
                        Console.WriteLine($"{_player2.username} used Flurry on {_player1.username}!");
                        if (!_player1.isBlocking)
                        {
                            int _timesHit = random.Next(2, 6);
                            Console.WriteLine($"Hit {_timesHit} times!");
                            _player1.currentHP -= (_timesHit * 15);
                            Console.WriteLine($"HP left: {_player1.currentHP}");
                        }
                        else
                        {
                            Console.WriteLine("...But they blocked it!");
                        }
                        break;
                }
                _player1.isBlocking = false;
                _player2.isBlocking = false;
                ServerSend.UpdatePlayer(_player1ID, _player1);
                ServerSend.UpdatePlayer(_player1ID, _player2);
                ServerSend.UpdatePlayer(_player2ID, _player1);
                ServerSend.UpdatePlayer(_player2ID, _player2);
                if(_player1.currentHP <= 0) // TODO: tie possibility?
                {
                    Console.WriteLine($"GAME OVER -> {_player2.username} WINS!!!");
                }
                if(_player2.currentHP <= 0)
                {
                    Console.WriteLine($"GAME OVER -> {_player1.username} WINS!!!");
                }
            }
            return;
        }
    }
}
