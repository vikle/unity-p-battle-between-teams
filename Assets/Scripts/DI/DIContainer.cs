using System;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    [DefaultExecutionOrder(-1000)]
    public sealed class DIContainer : MonoBehaviour
    {
        static readonly Dictionary<Type, object> sr_container = new();

        public MonoBehaviour[] monoBehaviours;

        void Awake()
        {
            foreach (var obj in monoBehaviours)
            {
                sr_container[obj.GetType()] = obj;
            }
        }
        
        void Reset()
        {
            name = "DIContainer";
        }

        void OnDestroy()
        {
            sr_container.Clear();
        }

        public static void Resolve<T>(out T instance) where T : class, new()
        {
            instance = Resolve<T>();
        }
        
        public static T Resolve<T>() where T : class, new()
        {
            if (!TryGet<T>(out var obj))
            {
                obj = new T();
                Register(obj);
            }

            return obj;
        }

        public static void Register<T>(T obj) where T : class
        {
            sr_container[obj.GetType()] = obj;
        }

        public static bool TryGet<T>(out T obj) where T : class
        {
            if (sr_container.TryGetValue(typeof(T), out object raw_obj))
            {
                obj = (T)raw_obj;
                return true;
            }

            obj = null;
            return false;
        }
    };
}
