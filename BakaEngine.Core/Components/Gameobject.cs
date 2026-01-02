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

        public Gameobject? Parent;
        public readonly List<Gameobject> Children = new();

        public Gameobject(string name)
        {
            this.name = name;
            transform = AddComponent<Transform>();
        }

        public bool TryGetComponent<T>(out T? component) where T : Component
        {
            if (components.TryGetValue(typeof(T), out var value))
            {
                component = (T)value;
                return true;
            }

            component = null;
            return false;
        }

        public T? GetComponent<T>() where T : Component
        {
            components.TryGetValue(typeof(T), out var value);
            return value as T;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var type = typeof(T);

            if (components.ContainsKey(type))
                throw new InvalidOperationException($"Component {type.Name} already exists.");

            var component = new T
            {
                Gameobject = this
            };

            components[type] = component;
            return component;
        }

        public void AddChild(Gameobject child)
        {
            child.Parent = this;
            Children.Add(child);
        }

    }
}