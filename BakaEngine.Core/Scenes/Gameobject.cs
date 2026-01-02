using System;
using System.Collections.Generic;

using BakaEngine.Core.Components;
using BakaEngine.Core.Helpers;

namespace BakaEngine.Core
{
    public class Gameobject
    {
        public string name;

        public Dictionary<Type, object> components = new Dictionary<Type, object>();

        public Transform transform;

        public Gameobject(string name)
        {
            this.name = name;
            transform = GetComponent<Transform>();
        }

        public bool TryGetComponent<T>(out T? component) where T : class
        {
            if (components.TryGetValue(typeof(T), out var value))
            {
                component = value as T;
                return true;
            }
            component = null;
            return false;
        }

        public T GetComponent<T>() where T : class, new()
        {
            if (!components.TryGetValue(typeof(T), out var value))
            {
                value = new T();
                components[typeof(T)] = value;
            }
            return (T)value;
        }

    }
}