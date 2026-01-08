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

        public void Initialize(Shader shader, Mesh mesh, Texture texture, Material material)
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
                    Matrix4.CreateScale(transform.Scale)
                    * Matrix4.CreateFromQuaternion(transform.Rotation)
                    * Matrix4.CreateTranslation(transform.Position));

            //TODO: also implement specular textures.

            if (texture.Type == TextureType.texture_diffuse)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
            }

            shader.SetInt("material.diffuse", material.diffuse_textureunit);
            shader.SetInt("material.specular", material.specular_textureunit);
            shader.SetVector3("material.diffuseColor", material.diffusecolor);
            shader.SetVector3("material.specularColor", material.specularcolor);
            shader.SetFloat("material.shininess", material.shininess);

            GL.BindVertexArray(mesh.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public override void Update()
        {
            base.Update();

            Draw(Gameobject.transform);
        }
    }
}