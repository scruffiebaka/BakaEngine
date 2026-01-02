using System;
using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components;

public class DirectionalLight : Light
{
    public Vector3 direction = Vector3.Zero;
    public DirectionalLight(){}
}
