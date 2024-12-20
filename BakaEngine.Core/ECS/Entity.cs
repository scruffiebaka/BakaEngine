using System;
using System.Collections.Generic;

using BakaEngine.Core.ECS.Components;
using BakaEngine.Core.Helpers;

namespace BakaEngine.Core.ECS
{
    public class Entity
    {
        private Entity? parent;

        private string entityName;

        private List<Component> entityComponents = new List<Component>();

        public Transform transform;

        public Entity(string name)
        {
            entityName = name;
            transform = new Transform();
            this.AddComponent(transform);
        }

        public void AddComponent(Component component)
        {
            if (!HasComponent(component))
            {
                component.entity = this;
                entityComponents.Add(component);
            }
            else
            {
                Debug.Error("Cannot add component. Already exists.");
            }
        }

        public T? GetComponent<T>() where T : Component
        {
            foreach (Component component in entityComponents)
            {
                if(component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }
            return null;
        }

        public bool HasComponent(Component component)
        {
            foreach (Component comp in entityComponents)
            {
                if (comp.GetType() == component.GetType())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
