using System;
using System.Collections.Generic;

using BakaEngine.Core.ECS.Components;
using BakaEngine.Core.Helpers;

namespace BakaEngine.Core.ECS
{
    public class Entity
    {
        //private Entity? parent;

        public string entityName;

        public Dictionary<Type, object> entityComponents = new Dictionary<Type, object>();

        public Transform transform;

        public Entity(string name)
        {
            entityName = name;
            transform = GetComponent<Transform>();
        }

        public bool TryGetComponent<T>(out T? component) where T : class
        {
            if (entityComponents.TryGetValue(typeof(T), out var value))
            {
                component = value as T;
                return true;
            }
            component = null;
            return false;
        }

        public T GetComponent<T>() where T : class, new()
        {
            if (!entityComponents.TryGetValue(typeof(T), out var value))
            {
                value = new T();
                entityComponents[typeof(T)] = value;
            }
            return (T)value;
        }

    }
}