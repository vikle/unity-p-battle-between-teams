using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Controllers;
using Scorewarrior.Test.Views;
using Random = UnityEngine.Random;

namespace Scorewarrior.Test
{
	public sealed class Bootstrapper : MonoBehaviour
	{
		[SerializeField]GameObject[] _characters;
		[SerializeField]SpawnPoint[] _spawns;

		IBattlefield _battlefield;
        readonly Queue<SpawnCharacterTask> _spawnCharactersQueue = new(8);

#if UNITY_EDITOR
        void OnValidate()
        {
            for (int i = 0, i_max = _characters.Length; i < i_max; i++)
            {
                checkPrefab(ref _characters[i]);
            }
            
            return;
            
            static void checkPrefab(ref GameObject prefab)
            {
                if (prefab == null) return;
                if (prefab.GetComponent<ICharacterPrefab>() != null) return;
                prefab = null;
                Debug.LogError("ICharacterPrefab must be implemented");
            }
        }
#endif

        void Awake()
        {
            GameController.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(EGameState newState)
        {
            switch (newState)
            {
                case EGameState.Initiated:
                    InitBattlefield();
                    break;
                case EGameState.Starting:
                    PrepareBattlefield();
                    break;
                case EGameState.Finished: 
                    break;
                default: break;
            }
        }

        private void InitBattlefield()
        {
            GameController.Wait();
            
            _battlefield ??= new Battlefield();
            
            foreach (var character in _battlefield.GetAllCharacters())
            {
                GameObjectPool.Return(character.Prefab.GameObject);
            }
            
            _battlefield.Clear();
        }
        
        
        private void PrepareBattlefield()
		{
            var availablePrefabs = new List<GameObject>(_characters);

            for (int i = 0, i_max = _spawns.Length; i < i_max; i++)
            {
                var spawnPoint = _spawns[i];
                
                if (spawnPoint == null)
                {
                    Debug.LogWarning($"Spawn point N{i} in _spawns array is null");
                    continue;
                }

                if (availablePrefabs.Count > 0)
                {
                    int randomIndex = Random.Range(0, availablePrefabs.Count);
                    var selectedPrefab = availablePrefabs[randomIndex];
                    
                    availablePrefabs.RemoveAt(randomIndex);
                    
                    if (selectedPrefab == null)
                    {
                        Debug.LogWarning($"Character prefab N{i} in _characters array is null");
                        continue;
                    }
                    
                    _spawnCharactersQueue.Enqueue(new()
                    {
                        Prefab = selectedPrefab,
                        Position = spawnPoint.transform.position,
                        Battlefield = _battlefield,
                        Team = spawnPoint.Team,
                        Sector = spawnPoint.Sector
                    });
                }
                else
                {
                    Debug.LogWarning($"Not found available character prefab for '{spawnPoint.name}'");
                }
            }

            StartCoroutine(SpawnCharacters());
        }

        private IEnumerator SpawnCharacters()
        {
            while (_spawnCharactersQueue.Count > 0)
            {
                yield return null;
                var task = _spawnCharactersQueue.Dequeue();
                GameController.SpawnCharacter(task.Prefab, task.Position, task.Battlefield, task.Team, task.Sector);
            }
            
            yield return null;
            GameController.Free();
        }
    }
}