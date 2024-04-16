using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectServer
{
    class InconsistencyInjection
    {
        Random rng = new Random();

        public (int, int) AlterInitialState(Player _player)
        {
            int maxHP = _player.maxHP;
            int numberPotions = _player.numberPotions;
            if (rng.Next(2) == 1)
            {
                maxHP += rng.Next(20);
                numberPotions += rng.Next(2);
            }
            else
            {
                maxHP -= rng.Next(20);
                numberPotions-= rng.Next(2);
            }
            return (maxHP, numberPotions);
            
        }

        public Player AlterDamage(Player _player, int _dmg, int _originalHP)
        {
            Console.WriteLine($"Damage dealt this turn pre corruption to Player {_player.id}: {_dmg}");
            Player corruptedPlayer = new Player(_player.id, _player.username, _player.maxHP, _player.numberPotions, _player.hasWon, _player.currentHP, _player.defense, _player.timesHit, _player.currentMove);
            if (rng.Next(2) == 0)
            {
                if (rng.Next(2) == 1)
                {
                    _dmg += rng.Next(10);
                }
                else
                {
                    _dmg -= rng.Next(10);
                }
            }
            corruptedPlayer.currentHP = _originalHP - _dmg;
            Console.WriteLine($"Damage dealt this turn post corruption to Player {_player.id}: {_dmg}");
            return corruptedPlayer;
        }
    }
}
