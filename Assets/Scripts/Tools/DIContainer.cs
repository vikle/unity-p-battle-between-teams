using System;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
[DefaultExecutionOrder(-100)]
public sealed class DIContainer : MonoBehaviour
{
    static readonly Dictionary<Type, object> sr_container = new();

    void Reset()
    {
        name = "DIContainer";
    }

    void OnDestroy()
    {
        sr_container.Clear();
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
