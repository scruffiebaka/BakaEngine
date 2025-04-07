using BakaEngine.Core.ECS.Components;
using BakaEngine.Core.ECS;

namespace BakaEngine.Core.ECS.Scenes
{
    public class Scene
    {
        string Name;
        public Camera? currentActiveCamera;

        public Scene(string name)
        {
            this.Name = name;
        }

        public List<Entity> entities = new List<Entity>();

        public void SetActiveCamera(Camera camera)
        {
            currentActiveCamera = camera;
        }
    }
}