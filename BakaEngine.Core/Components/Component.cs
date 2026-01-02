using System;

namespace BakaEngine.Core
{
    public class Component
    {
        public Gameobject Gameobject { get; internal set; }

        public virtual void Update()
        {
            
        }
    }
}