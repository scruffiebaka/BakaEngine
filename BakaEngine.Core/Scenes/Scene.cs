using System;
using System.Collections.Generic;

using BakaEngine.Core.ECS;

namespace BakaEngine.Core.Scenes
{
    public class Scene
    {
        string Name;

        public Scene(string name)
        {
            this.Name = name;
        }

        public List<Entity> entities = new List<Entity>();
    }
}
