using System;
using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components;

public abstract class Light
{
    public Vector3 ambient = new Vector3(0.05f, 0.05f, 0.05f);
    public Vector3 diffuse =  new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 specular = new Vector3(0.4f, 0.4f, 0.4f);
}
