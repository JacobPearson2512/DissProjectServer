using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void MarkerReceived(int _fromClient, Packet _packet) 
        {
            // recordstate, send marker
            Console.WriteLine(_packet.ReadString());
            Player _player1 = Server.clients[1].player;
            Player _player2 = Server.clients[2].player;
            Program.snapshotManager.snapshotId += 1;
            Snapshot snapshot = Program.snapshotManager.TakeSnapshot(Program.snapshotManager.snapshotId, new GlobalState(_player1.currentHP, _player2.currentHP, _player1.defense, _player2.defense, _player1.numberPotions, _player2.numberPotions));
            if (snapshot != null)
            {
                Console.WriteLine($"[Client [{_fromClient}] Snapshot {snapshot.snapshotId}:\nPlayer 1: <HP: {snapshot.state.player1Health}, Defense: {snapshot.state.player1Defense}, Potions: {snapshot.state.player1Potions}>\n" +
                    $"Player 2: <HP: {snapshot.state.player2Health}, Defense: {snapshot.state.player2Defense}, Potions: {snapshot.state.player2Potions}>");
            }

            ServerSend.Marker(_fromClient);
            
        }
    }
}
