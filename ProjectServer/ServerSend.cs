using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServer
{
    class ServerSend
    {
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

        public static void Welcome (int _toClient, string _msg)
        {
            Packet _packet = new Packet((int)ServerPackets.welcome);
            _packet.Write(_msg);
            _packet.Write(_toClient);
            SendTCPData(_toClient, _packet);
        }

        public static void UDPTest(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.udpTest))
            {
                _packet.Write("Hi! I'm a UDP test packet!");

                SendUDPData(_toClient, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.maxHP);
                _packet.Write(_player.numberPotions);

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
    }
}
