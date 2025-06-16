using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scorewarrior.Test.Services
{
    public sealed class UpdateEngine : MonoBehaviour
    {
        static readonly List<IUpdateHandler> sr_updateHandlers = new(64);
        static int s_updateHandlersCount;
        static UpdateEngine s_instance;
        
        static readonly List<string> sr_updateHandlersNames = new(64);
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            var thisInstance = new GameObject(nameof(UpdateEngine), typeof(UpdateEngine));
            DontDestroyOnLoad(thisInstance);
        }
        
        public static void RegisterHandler(IUpdateHandler handler)
        {
            sr_updateHandlers.Add(handler);
            s_updateHandlersCount++;
            
            sr_updateHandlersNames.Add(handler.GetType().FullName);
        }
        
        public static void UnregisterHandler(IUpdateHandler handler)
        {
            sr_updateHandlers.Remove(handler);
            s_updateHandlersCount--;
            
            sr_updateHandlersNames.Remove(handler.GetType().FullName);
        }

        void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            
            for (int i = 0; i < s_updateHandlersCount; i++)
            {
                Profiler.BeginSample(sr_updateHandlersNames[i]);
                sr_updateHandlers[i].OnUpdate(deltaTime);
                Profiler.EndSample();
            }
        }
    }
}
