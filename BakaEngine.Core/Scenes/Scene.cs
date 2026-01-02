using BakaEngine.Core.Components;
using BakaEngine.Core;

namespace BakaEngine.Core.Scenes
{
    public class Scene
    {
        string Name;
        public Gameobject? currentActiveCamera;

        public Scene(string name)
        {
            this.Name = name;
        }

        public List<Gameobject> entities = new List<Gameobject>();
        public List<Light> lights = new List<Light>();

        public void SetActiveCamera(Gameobject camera)
        {
            currentActiveCamera = camera;
        }
    }
}