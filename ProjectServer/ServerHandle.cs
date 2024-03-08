using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServer
{
    class ServerHandle
    {
        public static Queue<(int, string)> moveQueue = new Queue<(int, string)>();

        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientId = _packet.ReadInt();
            string _username = _packet.ReadString();
            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientId)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientId})!");
            }
            Console.WriteLine(_username);
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();
            Console.WriteLine($"Received via UDP. Contains: {_msg}");
        }

        public static void MoveSelected(int _fromClient, Packet _packet)
        {
            string _move = _packet.ReadString();
            moveQueue.Enqueue((_fromClient, _move));
            Server.clients[_fromClient].player.currentMove = _move;
            Console.WriteLine($"UDP: Client {_fromClient} used move: {_move}");
            if (moveQueue.Count == 2)
            {
                GameLogic.HandleAction();
            }
        }
    }
}
