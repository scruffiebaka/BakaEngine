using SharpGLTF.Schema2;
using System.Numerics;
using BakaEngine.Core.Components;
using BakaEngine.Core.Rendering;
using BakaEngine.Core.Scenes;

namespace BakaEngine.Core
{
    public static class GLTFImporter
    {
        public static Gameobject LoadModel(string modelPath, Shader shader)
        {
            Gameobject gameobject = new Gameobject("model");
            var model = ModelRoot.Load(modelPath);

            var scene = model.DefaultScene;

            SceneManager.currentActiveScene.gameobjects.Add(gameobject);

            foreach (var node in scene.VisualChildren)
            {
                ImportNode(node, gameobject, shader);
            }

            return gameobject;
        }

        static void ImportNode(Node node, Gameobject parent, Shader shader)
        {
            Matrix4x4 local = node.LocalMatrix;

            Gameobject go = new Gameobject(node.Name ?? "Node");

            Matrix4x4.Decompose(
                local,
                out Vector3 scale,
                out Quaternion rotation,
                out Vector3 translation
            );

            go.transform.LocalPosition = new OpenTK.Mathematics.Vector3(translation.X, translation.Y, translation.Z);
            go.transform.LocalRotation = new OpenTK.Mathematics.Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);
            go.transform.LocalScale = new OpenTK.Mathematics.Vector3(scale.X, scale.Y, scale.Z);

            SceneManager.currentActiveScene.gameobjects.Add(go);
            parent.AddChild(go);

            if (node.Mesh != null)
                ImportMesh(node.Mesh, go, shader);

            foreach (var child in node.VisualChildren)
                ImportNode(child, go, shader);
        }

        static void ImportMesh(SharpGLTF.Schema2.Mesh mesh, Gameobject owner, Shader shader)
        {
            foreach (var primitive in mesh.Primitives)
            {
                Gameobject primitiveobject = new Gameobject($"{owner.name}_Prim");
                owner.AddChild(primitiveobject);
                SceneManager.currentActiveScene.gameobjects.Add(primitiveobject);

                var positionAccessor = primitive.GetVertexAccessor("POSITION");
                var normalAccessor = primitive.GetVertexAccessor("NORMAL");
                var texAccessor = primitive.GetVertexAccessor("TEXCOORD_0");
                var indexAccessor = primitive.GetIndexAccessor();

                var positions = positionAccessor.AsVector3Array().ToArray();
                var normals = normalAccessor?.AsVector3Array().ToArray();
                var uvs = texAccessor?.AsVector2Array().ToArray();
                var indices = indexAccessor.AsIndicesArray().ToArray();

                List<Vertex> vertices = new();
                List<uint> indicesList = indices.ToList();

                for (int i = 0; i < positions.Length; i++)
                {
                    Vertex v = new Vertex();

                    v.Position.X = positions[i].X;
                    v.Position.Y = positions[i].Y;
                    v.Position.Z = positions[i].Z;

                    if (normals != null)
                    {
                        v.Normal.X = normals[i].X;
                        v.Normal.Y = normals[i].Y;
                        v.Normal.Z = normals[i].Z;
                    }

                    if (uvs != null)
                    {
                        v.TexCoords.X = uvs[i].X;
                        v.TexCoords.Y = 1.0f - uvs[i].Y;
                    }

                    vertices.Add(v);
                }

                SharpGLTF.Schema2.Material? mat = primitive.Material;
                MaterialChannel? baseColor = mat.FindChannel("BaseColor");
                SharpGLTF.Schema2.Texture? texture = null;
                if (baseColor != null)
                {
                    texture = baseColor.Value.Texture;
                }

                Rendering.Texture baseTex;

                if (texture != null)
                {
                    byte[] content = texture.PrimaryImage.Content.Content.ToArray();
                    baseTex = new Rendering.Texture(
                        Rendering.Texture.LoadFromMemory(content),
                        TextureType.texture_diffuse
                    );
                }
                else
                {
                    baseTex = new Rendering.Texture(
                        Rendering.Texture.LoadFromFile("Resources/Textures/white.png"),
                        TextureType.texture_diffuse
                    );
                }

                Rendering.Material baseMat = new Rendering.Material(
                    new OpenTK.Mathematics.Vector3(
                        baseColor.Value.Color.X,
                        baseColor.Value.Color.Y,
                        baseColor.Value.Color.Z),
                    OpenTK.Mathematics.Vector3.One,
                    0, 1,
                    32.0f);

                var meshComp = primitiveobject.AddComponent<Components.Mesh>();
                meshComp.Initialize(vertices, indicesList, shader);

                var renderer = primitiveobject.AddComponent<Components.MeshRenderer>();
                renderer.Initialize(shader, meshComp, baseTex, baseMat);
            }
        }

    }
}
