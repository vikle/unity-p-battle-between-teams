using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public static class GameObjectPool
    {
        static readonly Dictionary<GameObject, Stack<GameObject>> sr_prefabToInstance = new(64);
        static readonly Dictionary<GameObject, GameObject> sr_instanceToPrefab = new(64);
        static Scene s_poolScene;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            s_poolScene = SceneManager.CreateScene(nameof(GameObjectPool));
        }

        public static GameObject Instantiate(GameObject prefab)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Instantiate(GameObject prefab, Vector3 position)
        {
            return Instantiate(prefab, position, Quaternion.identity);
        }

        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject instance;

            if (sr_prefabToInstance.TryGetValue(prefab, out var stack))
            {
                instance = (stack.Count > 0) 
                    ? stack.Pop() 
                    : CreateInstance(prefab);
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
            
            if (!sr_prefabToInstance.TryGetValue(prefab, out var stack))
            {
                stack = new Stack<GameObject>();
                sr_prefabToInstance[prefab] = stack;
            }
            
            stack.Push(instance);
            instance.SetActive(false);
        }

        public static bool ContainsPrefab(GameObject prefab)
        {
            return sr_prefabToInstance.ContainsKey(prefab);
        }

        public static bool ContainsInstance(GameObject instance)
        {
            return sr_instanceToPrefab.ContainsKey(instance);
        }
    };
}
