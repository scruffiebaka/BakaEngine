using System.Collections.Generic;
using BakaEngine.Core.Scenes;
using BakaEngine.Core.Helpers;
using BakaEngine.Core.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BakaEngine.Core.Components
{
    public class MeshRenderer : Component
    {
        Shader shader;
        Mesh mesh;
        Texture texture;
        Material material;

        //uhh test
        Vector3 defaultAttenuation = new Vector3(1.0f, 0.09f, 0.032f);

        public MeshRenderer(Shader shader, Mesh mesh, Texture texture, Material material)
        {
            this.shader = shader;
            this.mesh = mesh;
            this.texture = texture;
            this.material = material;
        }

        public void Draw(Transform transform)
        {
            shader.Use();

            shader.SetMatrix4("model",
                Matrix4.CreateTranslation(transform.Position)
                * Matrix4.CreateScale(transform.Scale)
                * Matrix4.CreateFromQuaternion(transform.Rotation));

            //TODO: also implement specular textures.

            if (texture.Type == TextureType.texture_diffuse)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
            }

            shader.SetInt("material.diffuse", material.diffuse);
            shader.SetInt("material.specular", material.specular);
            shader.SetFloat("material.shininess", material.shininess);

            CalculateLighting();

            GL.BindVertexArray(mesh.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public void CalculateLighting()
        {
            int n_directionallights = 0;
            int n_pointlights = 0;
            int n_spotlights = 0;

            if (SceneManager.currentActiveScene != null)
            {
                foreach (Light light in SceneManager.currentActiveScene.lights)
                {
                    if (light is DirectionalLight)
                    {
                        shader.SetVector3($"dirLights[{n_directionallights}].direction", ((DirectionalLight)light).direction);
                        shader.SetVector3($"dirLights[{n_directionallights}].ambient", light.ambient);
                        shader.SetVector3($"dirLights[{n_directionallights}].diffuse", light.diffuse);
                        shader.SetVector3($"dirLights[{n_directionallights}].specular", light.specular);
                        n_directionallights++;
                    }
                    else if (light is Pointlight)
                    {
                        shader.SetVector3($"pointLights[{n_pointlights}].position", ((Pointlight)light).position);
                        shader.SetVector3($"pointLights[{n_pointlights}].ambient", light.ambient);
                        shader.SetVector3($"pointLights[{n_pointlights}].diffuse", light.diffuse);
                        shader.SetVector3($"pointLights[{n_pointlights}].specular", light.specular);
                        shader.SetFloat($"pointLights[{n_pointlights}].constant", defaultAttenuation.X);
                        shader.SetFloat($"pointLights[{n_pointlights}].linear", defaultAttenuation.Y);
                        shader.SetFloat($"pointLights[{n_pointlights}].quadratic", defaultAttenuation.Z);
                        n_pointlights++;
                    }
                    else if (light is Spotlight)
                    {
                        shader.SetVector3($"spotLights[{n_spotlights}].position", ((Spotlight)light).position);
                        shader.SetVector3($"spotLights[{n_spotlights}].direction", ((Spotlight)light).direction);
                        shader.SetVector3($"spotLights[{n_spotlights}].ambient", light.ambient);
                        shader.SetVector3($"spotLights[{n_spotlights}].diffuse", light.diffuse);
                        shader.SetVector3($"spotLights[{n_spotlights}].specular", light.specular);
                        shader.SetFloat($"spotLights[{n_spotlights}].constant", defaultAttenuation.X);
                        shader.SetFloat($"spotLights[{n_spotlights}].linear", defaultAttenuation.Y);
                        shader.SetFloat($"spotLights[{n_spotlights}].quadratic", defaultAttenuation.Z);

                        float outer = MathF.Cos(MathHelper.DegreesToRadians(((Spotlight)light).spotAngle));
                        float inner = MathF.Cos(MathHelper.DegreesToRadians(((Spotlight)light).spotAngle * 0.8f));

                        shader.SetFloat($"spotLights[{n_spotlights}].cutOff", inner);
                        shader.SetFloat($"spotLights[{n_spotlights}].outerCutOff", outer);
                        n_spotlights++;
                    }
                }
            }

            for (int i = n_directionallights; i < 2; i++)
            {
                shader.SetVector3($"dirLights[{i}].direction", Vector3.Zero);
                shader.SetVector3($"dirLights[{i}].ambient", Vector3.Zero);
                shader.SetVector3($"dirLights[{i}].diffuse", Vector3.Zero);
                shader.SetVector3($"dirLights[{i}].specular", Vector3.Zero);
            }

            for (int i = n_pointlights; i < 16; i++)
            {
                shader.SetVector3($"pointLights[{i}].position", Vector3.Zero);
                shader.SetVector3($"pointLights[{i}].ambient", Vector3.Zero);
                shader.SetVector3($"pointLights[{i}].diffuse", Vector3.Zero);
                shader.SetVector3($"pointLights[{i}].specular", Vector3.Zero);
                shader.SetFloat($"pointLights[{i}].constant", 1.0f);
                shader.SetFloat($"pointLights[{i}].linear", 0.0f);
                shader.SetFloat($"pointLights[{i}].quadratic", 0.0f);
            }

            for (int i = n_spotlights; i < 8; i++)
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