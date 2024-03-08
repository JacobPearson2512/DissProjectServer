using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO.Pipes;

namespace ProjectServer
{
    class Server
    {
        public static int PlayerLimit { get; private set; }
        public static int Port {  get; private set; }

        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static int connectedPlayers = 0;

        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;


        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void Start(int _PlayerLimit,  int _Port)
        {
            PlayerLimit = _PlayerLimit;
            Port = _Port;

            InitialiseServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            udpListener = new UdpClient(Port); // i dont like this. Change to client, consistency.
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Console.WriteLine($"Server has started on Port: {Port}!");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");


            for (int i = 1; i <= PlayerLimit; i++) 
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server is at capacity.");

        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length < 4)
                {
                    //todo same issue.
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientID = _packet.ReadInt();
                    if (_clientID == 0)
                    {
                        return;
                    }

                    if (clients[_clientID].udp.endPoint == null)
                    {
                        clients[_clientID].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientID].udp.endPoint.ToString() == _clientEndPoint.ToString()) // protects against hackers: cant just use another endpoint.
                    {
                        clients[_clientID].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving UDP data: {e}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {e}");
            }
        }

        private static void InitialiseServerData()
        {
            for (int i = 1; i < PlayerLimit; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived},
                { (int)ClientPackets.udpTestReceived, ServerHandle.UDPTestReceived},
                { (int)ClientPackets.moveSelected, ServerHandle.MoveSelected}
            };
            Console.WriteLine("Initialised Packets.");


        }
    }
}
