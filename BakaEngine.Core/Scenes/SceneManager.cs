using System;
using System.Collections.Generic;

using BakaEngine.Core.ECS.Components;
using BakaEngine.Core.Helpers;

namespace BakaEngine.Core.Scenes
{
    public sealed class SceneManager
    {
        private static readonly Lazy<SceneManager> lazy =
            new Lazy<SceneManager>(() => new SceneManager());

        public static SceneManager Instance { get { return lazy.Value; } }

        private SceneManager()
        {
        }

        public static Scene? currentActiveScene;

        private static List<Scene> scenes = new List<Scene>();

        public static Scene? GetScene(int SceneID)
        {
            try
            {
                return scenes.ElementAt(SceneID);
            }
            catch
            {
                Debug.Error("Scene at SceneID not found");
                return null;
            }
        }

        public static void SetActiveScene(Scene scene)
        {
            currentActiveScene = scene;
        }
    }
}
