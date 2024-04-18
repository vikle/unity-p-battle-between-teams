using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scorewarrior.Test
{
    public static class GameObjectPool
    {
        static readonly Dictionary<GameObject, Stack<GameObject>> sr_prefabToInstance = new(64);
        static readonly Dictionary<GameObject, GameObject> sr_instanceToPrefab = new(64);
        static Scene s_poolScene;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
            => s_poolScene = SceneManager.CreateScene(nameof(GameObjectPool));

        public static GameObject Instantiate(GameObject prefab)
            => Instantiate(prefab, Vector3.zero, Quaternion.identity);
        
        public static GameObject Instantiate(GameObject prefab, Vector3 position)
            => Instantiate(prefab, position, Quaternion.identity);
        
        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject instance;

            if (sr_prefabToInstance.TryGetValue(prefab, out var stack))
            {
                instance = stack.Count > 0 ? stack.Pop() : CreateInstance(prefab);
            }
            else
            {
                instance = CreateInstance(prefab);
                sr_prefabToInstance[prefab] = new Stack<GameObject>();
            }
            
            var tr = instance.transform;
            tr.position = position;
            tr.rotation = rotation;
            
            instance.SetActive(true);
            
            return instance;
        }
        
        private static GameObject CreateInstance(GameObject prefab)
        {
            var instance = Object.Instantiate(prefab);
            sr_instanceToPrefab[instance] = prefab;
            SceneManager.MoveGameObjectToScene(instance, s_poolScene);
            return instance;
        }
        
        public static void Return(GameObject instance)
        {
            var prefab = sr_instanceToPrefab[instance];
            
            if (sr_prefabToInstance.TryGetValue(prefab, out var stack) == false)
            {
                stack = new Stack<GameObject>();
                sr_prefabToInstance[prefab] = stack;
            }
            
            stack.Push(instance);
            instance.SetActive(false);
        }

        public static bool ContainsPrefab(GameObject prefab)
            => sr_prefabToInstance.ContainsKey(prefab);
        
        public static bool ContainsInstance(GameObject instance)
            => sr_instanceToPrefab.ContainsKey(instance);
    };
}
