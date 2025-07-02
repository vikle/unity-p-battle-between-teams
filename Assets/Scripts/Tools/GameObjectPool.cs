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
        static Dictionary<GameObject, Stack<GameObject>> s_prefabToInstance;
        static Dictionary<GameObject, GameObject> s_instanceToPrefab;
        static Scene s_scene;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            s_prefabToInstance = new(16);
            s_instanceToPrefab = new(16);
            s_scene = SceneManager.CreateScene("_GameObjectPoolScene");
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

            if (s_prefabToInstance.TryGetValue(prefab, out var stack))
            {
                instance = (stack.Count > 0) 
                    ? stack.Pop() 
                    : CreateInstance(prefab);
            }
            else
            {
                instance = CreateInstance(prefab);
                s_prefabToInstance[prefab] = new Stack<GameObject>();
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
            s_instanceToPrefab[instance] = prefab;
            SceneManager.MoveGameObjectToScene(instance, s_scene);
            return instance;
        }
        
        public static void Return(GameObject instance)
        {
            var prefab = s_instanceToPrefab[instance];
            
            if (!s_prefabToInstance.TryGetValue(prefab, out var stack))
            {
                stack = new Stack<GameObject>();
                s_prefabToInstance[prefab] = stack;
            }
            
            stack.Push(instance);
            instance.SetActive(false);
        }

        public static bool ContainsPrefab(GameObject prefab)
        {
            return s_prefabToInstance.ContainsKey(prefab);
        }

        public static bool ContainsInstance(GameObject instance)
        {
            return s_instanceToPrefab.ContainsKey(instance);
        }
    };
}
