using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServer
{
    class InconsistencyResolution
    {
        GlobalState client1State;
        GlobalState client2State;
        GlobalState serverState;

        public InconsistencyResolution(GlobalState _client1State, GlobalState _client2State, GlobalState _serverState)
        {
            client1State = _client1State;
            client2State = _client2State;
            serverState = _serverState;
        }

        public GlobalState Consensus()
        {
            Console.WriteLine("Initiating Consensus Algorithm");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Server State: [{serverState.player1Health}, {serverState.player2Health}, {serverState.player1Defense}, {serverState.player2Defense}, {serverState.player1Potions}, {serverState.player2Potions}]");
            Console.WriteLine($"Client 2 State: [{client2State.player1Health}, {client2State.player2Health}, {client2State.player1Defense}, {client2State.player2Defense}, {client2State.player1Potions}, {client2State.player2Potions}]");
            Console.WriteLine($"Client 1 State: [{client1State.player1Health}, {client1State.player2Health}, {client1State.player1Defense}, {client1State.player2Defense}, {client1State.player1Potions}, {client1State.player2Potions}]");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            if (client1State.player1Health == client2State.player1Health && client1State.player2Health == client2State.player2Health && client1State.player1Defense == client2State.player1Defense && client1State.player2Defense == client2State.player2Defense && client1State.player1Potions == client2State.player1Potions && client1State.player2Potions == client2State.player2Potions)
            {
                Console.WriteLine($"[{client1State.player1Health}, {client1State.player2Health}, {client1State.player1Defense}, {client1State.player2Defense}, {client1State.player1Potions}, {client1State.player2Potions}] is the chosen state.");
                return client1State;
            }
            else if (client2State.player1Health == serverState.player1Health && client2State.player2Health == serverState.player2Health && client2State.player1Defense == serverState.player1Defense && client2State.player2Defense == serverState.player2Defense && client2State.player1Potions == serverState.player1Potions && client2State.player2Potions == serverState.player2Potions)
            {
                Console.WriteLine($"[{client2State.player1Health}, {client2State.player2Health}, {client2State.player1Defense}, {client2State.player2Defense}, {client2State.player1Potions}, {client2State.player2Potions}] is the chosen state.");
                return client2State;
            }
            else
            {
                Console.WriteLine($"[{serverState.player1Health}, {serverState.player2Health}, {serverState.player1Defense}, {serverState.player2Defense}, {serverState.player1Potions}, {serverState.player2Potions}] is the chosen state");
                return serverState;
            }
        }
    }
}
