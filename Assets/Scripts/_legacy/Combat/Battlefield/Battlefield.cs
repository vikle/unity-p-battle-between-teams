using System.Linq;
using System.Collections.Generic;

namespace Scorewarrior.Test.Models
{
    public sealed class Battlefield : IBattlefield
    {
        readonly List<ICharacter> _allCharacters = new(16);
        readonly Dictionary<ETeam, List<ICharacter>> _charactersByTeam = new(4);

        public void Clear()
        {
            _allCharacters.Clear();
            _charactersByTeam.Clear();
        }
        
        public void RegisterCharacter(ICharacter character)
        {
            if (_allCharacters.Contains(character) == false)
            {
                _allCharacters.Add(character);
            }

            if (_charactersByTeam.TryGetValue(character.Team, out var charactersList))
            {
                if (charactersList.Contains(character) == false)
                {
                    charactersList.Add(character);
                }
            }
            else
            {
                _charactersByTeam.Add(character.Team, new(){character});
            }
        }
        
        public bool TryGetNearestAliveEnemy(ICharacter character, out ICharacter target)
        {
            target = null;
            float nearestDistance = float.MaxValue;
            
            for (int i = 0, i_max = _allCharacters.Count; i < i_max; i++)
            {
                var other = _allCharacters[i];
                if (!other.IsAlive) continue;
                if (other == character) continue;

                // Каждый перс теперь хранит в себе инфу о своей тиме, что позволило сильно оптимизировать логику
                if (other.Team == character.Team) continue;

                var direction = (character.Prefab.Transform.position - other.Prefab.Transform.position);
                float distance = direction.sqrMagnitude;
                //Убираем Vector3.Distance, т.к. в данном случае вычисление квадратного корня не нужно

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    target = other;
                }
            }

            return (target != null);
        }

        public ICharacter[] GetAllCharacters()
            => _allCharacters.ToArray();
        
        public ICharacter[] GetCharactersOfTeam(ETeam team)
            => _charactersByTeam[team].ToArray();

        public bool TeamIsAlive(ETeam team)
            => _charactersByTeam[team].Any(chr => chr.IsAlive);
        
        public bool TeamIsDead(ETeam team)
            => _charactersByTeam[team].All(chr => !chr.IsAlive);
    }
}
