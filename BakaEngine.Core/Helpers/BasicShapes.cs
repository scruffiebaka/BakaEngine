using BakaEngine.Core.Components;
using OpenTK.Mathematics;

namespace BakaEngine.Core.Helpers;

public static class BasicShapes
{
    public static readonly List<Vertex> CUBE_VERTICES = new List<Vertex> {
            // Front face
            new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(0, 1) },

            // Back face
            new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(0, 1) },

            // Left face
            new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(0, 1) },

            // Right face
            new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(0, 1) },

            // Top face
            new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(0, 1) },

            // Bottom face
            new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(0, 1) },
    };
    public static readonly List<uint> CUBE_INDICES = new List<uint>{
            // Front face
            0, 1, 2, 2, 3, 0,
            // Back face
            4, 5, 6, 6, 7, 4,
            // Left face
            8, 9, 10, 10, 11, 8,
            // Right face
            12, 13, 14, 14, 15, 12,
            // Top face
            16, 17, 18, 18, 19, 16,
            // Bottom face
            20, 21, 22, 22, 23, 20
    };
}