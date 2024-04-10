using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServer
{
    class InconsistencyEvaluation
    {
        // TODO: INSTEAD OF PLAYERS THIS SHOULD BE CLIENTS. REQUEST PACKETS ETC.
        public class LocalInconsistency
        {
            public int id;
            public Client playerI;
            public Client playerJ;
            public int value = 0;

            public LocalInconsistency(int _id, Client _playerI, Client _playerJ)
            {
                id = _id;
                playerI = _playerI;
                playerJ = _playerJ;
            }

            public int Calculate()
            {
                bool sameStates;
                if ((playerI.initialState.player1Health == playerJ.initialState.player1Health) && (playerI.initialState.player2Health == playerJ.initialState.player2Health) && (playerI.initialState.player1Defense == playerJ.initialState.player1Defense) && (playerI.initialState.player2Defense == playerJ.initialState.player2Defense) && (playerI.initialState.player1Potions == playerJ.initialState.player1Potions) && (playerI.initialState.player2Potions == playerJ.initialState.player2Potions))
                {
                    sameStates = true;
                } // TODO: make better
                else
                {
                    sameStates=false;
                    Console.WriteLine("Player states were unmatched to begin with.");
                    value += 1;
                }
                if (playerI.winningPlayerID != playerJ.winningPlayerID)
                {
                    Console.WriteLine("Players have different recorded winners.");
                    value += 1;
                }
                return value;
            }
        }

        public int CalculateTotalInconsistency(List<LocalInconsistency> localInconsistencies)
        {
            int total = 0;
            for (int i = 0; i < localInconsistencies.Count; i++)
            {
                total += localInconsistencies[i].value;
            }
            return total;
        }

        public class GlobalInconsistency
        {
            public int total = 0;
            public GlobalState serverFinalState = Program.snapshotManager.getFinalState(); 

            public int CompareEndState()
            {
                int stateDistance = 0;
                foreach (Client _client in Server.clients.Values)
                {
                    int _clientID = _client.id; 
                    if (_client.finalState.player1Health != serverFinalState.player1Health)
                    {
                        stateDistance++;
                    }
                    if (_client.finalState.player2Health != serverFinalState.player2Health)
                    {
                        stateDistance++;
                    }
                    if (_client.finalState.player1Defense != serverFinalState.player1Defense)
                    {
                        stateDistance++;
                    }
                    if (_client.finalState.player2Defense != serverFinalState.player2Defense)
                    {
                        stateDistance++;
                    }
                    if (_client.finalState.player1Potions != serverFinalState.player1Potions)
                    {
                        stateDistance++;
                    }
                    if (_client.finalState.player2Potions != serverFinalState.player2Potions)
                    {
                        stateDistance++;
                    }
                }
                for (int i = 0; i<Server.clients.Count; i++)
                {
                    for (int j = i + 1; j<Server.clients.Count; j++)
                    {
                        Console.WriteLine($"Client {Server.clients[i + 1].id} vs {Server.clients[j + 1].id}");
                        if (Server.clients[i + 1].finalState.player1Health != Server.clients[j + 1].finalState.player1Health)
                        {
                            stateDistance++;
                        }
                        if (Server.clients[i + 1].finalState.player2Health != Server.clients[j + 1].finalState.player2Health)
                        {
                            stateDistance++;
                        }
                        if (Server.clients[i + 1].finalState.player1Defense != Server.clients[j + 1].finalState.player1Defense)
                        {
                            stateDistance++;
                        }
                        if (Server.clients[i + 1].finalState.player2Defense != Server.clients[j + 1].finalState.player2Defense)
                        {
                            stateDistance++;
                        }
                        if (Server.clients[i + 1].finalState.player1Potions != Server.clients[j + 1].finalState.player1Potions)
                        {
                            stateDistance++;
                        }
                        if (Server.clients[i + 1].finalState.player2Potions != Server.clients[j + 1].finalState.player2Potions)
                        {
                            stateDistance++;
                        }
                    }
                }
                return stateDistance;
                // Snapshot record, get end state of server and client
                // calculate difference between the two, levenshtein?
                // as of 7/4/24 - need inconsistency injection. 
            }

            public int CompareMoveEffectHistory()
            {
                int moveDifferences = 0;
                List<MoveHistoryEntry> client1P1Moves = new List<MoveHistoryEntry>();
                List<MoveHistoryEntry> client1P2Moves = new List<MoveHistoryEntry>();
                List<MoveHistoryEntry> client2P1Moves = new List<MoveHistoryEntry>();
                List<MoveHistoryEntry> client2P2Moves = new List<MoveHistoryEntry>();
                foreach (MoveHistoryEntry _entry in Server.clients[1].moveHistory)
                {
                    if (_entry.playerID == 1)
                    {
                        client1P1Moves.Add(_entry);
                    }
                    else
                    {
                        client1P2Moves.Add(_entry);
                    }
                }
                foreach (MoveHistoryEntry _entry in Server.clients[2].moveHistory)
                {
                    if (_entry.playerID == 1)
                    {
                        client2P1Moves.Add(_entry);
                    }
                    else
                    {
                        client2P2Moves.Add(_entry);
                    }
                }
                for (int i = 0; i < client1P1Moves.Count; i++)
                {
                    if ((client1P1Moves[i] != null) && (i<client2P1Moves.Count))
                    {
                        if (client1P1Moves[i].actionName != client2P1Moves[i].actionName)
                        {
                            Console.WriteLine($"Turn {i} has mismatched moves for Player 1.");
                            moveDifferences++;
                        }
                        if (client1P1Moves[i].actionEffect != client2P1Moves[i].actionEffect)
                        {
                            Console.WriteLine($"Turn {i} has mismatched move effects for Player 1.");
                            moveDifferences++;
                        }
                    }
                    if ((i<client1P2Moves.Count) && (i<client2P2Moves.Count))
                    {
                        if (client1P2Moves[i].actionName != client2P2Moves[i].actionName)
                        {
                            Console.WriteLine($"Turn {i} has mismatched moves for Player 2.");
                            moveDifferences++;
                        }
                        if (client1P2Moves[i].actionEffect != client2P2Moves[i].actionEffect)
                        {
                            Console.WriteLine($"Turn {i} has mismatched move effects for Player 2.");
                            moveDifferences++;
                        }
                    }
                    
                }
                
                return moveDifferences; 
            }

            public int Calculate()
            {
                total = CompareEndState() + CompareMoveEffectHistory();
                return total;
            }

        }
    }
}
