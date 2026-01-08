using BakaEngine.Core.Components;
using BakaEngine.Core;
using OpenTK.Mathematics;
using BakaEngine.Core.Rendering;

namespace BakaEngine.Core.Scenes
{
    public class Scene
    {
        string Name;
        public Gameobject? currentActiveCamera;

        Vector3 defaultAttenuation = new Vector3(1.0f, 0.09f, 0.032f);

        public Scene(string name)
        {
            this.Name = name;
        }

        public List<Gameobject> gameobjects = new List<Gameobject>();
        public List<Light> lights = new List<Light>();

        public void SetActiveCamera(Gameobject camera)
        {
            currentActiveCamera = camera;
        }

        public void CalculateLighting(Shader shader)
        {
            int n_directionallights = 0;
            int n_pointlights = 0;
            int n_spotlights = 0;

            if (SceneManager.currentActiveScene != null)
            {
                foreach (Light light in SceneManager.currentActiveScene.lights)
                {
                    if (light is DirectionalLight dl)
                    {
                        shader.SetVector3($"dirLights[{n_directionallights}].direction", dl.direction);
                        shader.SetVector3($"dirLights[{n_directionallights}].ambient", dl.ambient);
                        shader.SetVector3($"dirLights[{n_directionallights}].diffuse", dl.diffuse);
                        shader.SetVector3($"dirLights[{n_directionallights}].specular", dl.specular);
                        n_directionallights++;
                    }
                    else if (light is Pointlight pl)
                    {
                        shader.SetVector3($"pointLights[{n_pointlights}].position", pl.position);
                        shader.SetVector3($"pointLights[{n_pointlights}].ambient", pl.ambient);
                        shader.SetVector3($"pointLights[{n_pointlights}].diffuse", pl.diffuse);
                        shader.SetVector3($"pointLights[{n_pointlights}].specular", pl.specular);
                        shader.SetFloat($"pointLights[{n_pointlights}].constant", defaultAttenuation.X);
                        shader.SetFloat($"pointLights[{n_pointlights}].linear", defaultAttenuation.Y);
                        shader.SetFloat($"pointLights[{n_pointlights}].quadratic", defaultAttenuation.Z);
                        n_pointlights++;
                    }
                    else if (light is Spotlight sl)
                    {
                        shader.SetVector3($"spotLights[{n_spotlights}].position", sl.position);
                        shader.SetVector3($"spotLights[{n_spotlights}].direction", sl.direction);
                        shader.SetVector3($"spotLights[{n_spotlights}].ambient", sl.ambient);
                        shader.SetVector3($"spotLights[{n_spotlights}].diffuse", sl.diffuse);
                        shader.SetVector3($"spotLights[{n_spotlights}].specular", sl.specular);
                        shader.SetFloat($"spotLights[{n_spotlights}].constant", defaultAttenuation.X);
                        shader.SetFloat($"spotLights[{n_spotlights}].linear", defaultAttenuation.Y);
                        shader.SetFloat($"spotLights[{n_spotlights}].quadratic", defaultAttenuation.Z);

                        float outer = MathF.Cos(MathHelper.DegreesToRadians(sl.spotAngle));
                        float inner = MathF.Cos(MathHelper.DegreesToRadians(sl.spotAngle * 0.8f));

                        shader.SetFloat($"spotLights[{n_spotlights}].cutOff", inner);
                        shader.SetFloat($"spotLights[{n_spotlights}].outerCutOff", outer);
                        n_spotlights++;
                    }
                }
            }

            for (int i = n_directionallights; i < 1; i++)
            {
                shader.SetVector3($"dirLights[{i}].direction", Vector3.Zero);
                shader.SetVector3($"dirLights[{i}].ambient", Vector3.Zero);
                shader.SetVector3($"dirLights[{i}].diffuse", Vector3.Zero);
                shader.SetVector3($"dirLights[{i}].specular", Vector3.Zero);
            }

            for (int i = n_pointlights; i < 4; i++)
            {
                shader.SetVector3($"pointLights[{i}].position", Vector3.Zero);
                shader.SetVector3($"pointLights[{i}].ambient", Vector3.Zero);
                shader.SetVector3($"pointLights[{i}].diffuse", Vector3.Zero);
                shader.SetVector3($"pointLights[{i}].specular", Vector3.Zero);
                shader.SetFloat($"pointLights[{i}].constant", 1.0f);
                shader.SetFloat($"pointLights[{i}].linear", 0.0f);
                shader.SetFloat($"pointLights[{i}].quadratic", 0.0f);
            }

            for (int i = n_spotlights; i < 4; i++)
            {
                shader.SetVector3($"spotLights[{i}].position", Vector3.Zero);
                shader.SetVector3($"spotLights[{i}].direction", Vector3.Zero);
                shader.SetVector3($"spotLights[{i}].ambient", Vector3.Zero);
                shader.SetVector3($"spotLights[{i}].diffuse", Vector3.Zero);
                shader.SetVector3($"spotLights[{i}].specular", Vector3.Zero);
                shader.SetFloat($"spotLights[{i}].constant", 1.0f);
                shader.SetFloat($"spotLights[{i}].linear", 0.0f);
                shader.SetFloat($"spotLights[{i}].quadratic", 0.0f);
                shader.SetFloat($"spotLights[{i}].cutOff", 0.0f);
                shader.SetFloat($"spotLights[{i}].outerCutOff", 0.0f);
            }
        }
    }
}