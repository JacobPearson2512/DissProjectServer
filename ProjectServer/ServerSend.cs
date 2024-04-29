using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectServer
{
    class ServerSend
    {
        static InconsistencyInjection injection = new InconsistencyInjection();
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.PlayerLimit; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.PlayerLimit; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAllButOne(int _exceptClient,  Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.PlayerLimit; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAllButOne(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.PlayerLimit; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        public static void Welcome (int _toClient, string _msg, bool _inject)
        {
            Packet _packet = new Packet((int)ServerPackets.welcome);
            _packet.Write(_msg);
            _packet.Write(_toClient);
            _packet.Write(_inject);
            SendTCPData(_toClient, _packet);
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                int maxHP = _player.maxHP;
                int numberPotions = _player.numberPotions;
                if (Program.injectInconsistency)
                {
                    (int, int) HpPotions = injection.AlterInitialState(_player);
                    maxHP = HpPotions.Item1;
                    numberPotions = HpPotions.Item2;
                }
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(maxHP);
                _packet.Write(numberPotions);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void StartBattle(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.startBattle))
            {
                _packet.Write("2 Players have connected. Battle Begin!");

                SendTCPData(_toClient, _packet);
            }
        }

        public static void UpdatePlayer(int _toClient, Player _player)
        {
            using (Packet  _packet = new Packet((int)ServerPackets.updatePlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.currentHP);
                _packet.Write(_player.numberPotions);
                _packet.Write(_player.defense);
                _packet.Write(_player.currentMove);
                _packet.Write(_player.timesHit);
                _packet.Write(_player.hasWon);

                SendUDPData(_toClient, _packet);
            }
        }

        public static void Marker(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.marker))
            {
                _packet.Write("Marker arrived, initiate snapshot");
                SendTCPData(_toClient, _packet);
            }
        }

        public static void Consensus(int _toClient, GlobalState _majorityState)
        {
            using (Packet _packet = new Packet((int)ServerPackets.Consensus))
            {
                _packet.Write(_majorityState.player1Health);
                _packet.Write(_majorityState.player2Health);
                _packet.Write(_majorityState.player1Defense);
                _packet.Write(_majorityState.player2Defense);
                _packet.Write(_majorityState.player1Potions);
                _packet.Write(_majorityState.player2Potions);
                SendTCPData( _toClient, _packet);
            }
        }
    }
}
