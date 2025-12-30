using System.Collections.Generic;
using BakaEngine.Core.Helpers;
using BakaEngine.Core.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components
{
    public class MeshRenderer : Component
    {
        Shader shader;
        Mesh mesh; 
        Texture texture;
        Material material;

        //uhh test
        Vector3 _lightPos = new Vector3(1.0f, 1.0f, 2.0f);

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

            if(texture.Type == TextureType.texture_diffuse)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
            }
            
            shader.SetInt("material.diffuse", material.diffuse);
            shader.SetInt("material.specular", material.specular);
            shader.SetFloat("material.shininess", material.shininess);

            shader.SetVector3("light.position", _lightPos);
            shader.SetVector3("light.ambient", new Vector3(0.2f));
            shader.SetVector3("light.diffuse", new Vector3(0.5f));
            shader.SetVector3("light.specular", new Vector3(1.0f));

            GL.BindVertexArray(mesh.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

    }
}