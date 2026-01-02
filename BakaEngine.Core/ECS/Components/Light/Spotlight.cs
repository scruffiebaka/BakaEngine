using System;
using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components;

public class Spotlight : Light
{
    public Vector3 position = Vector3.Zero;
    public Vector3 direction = Vector3.Zero;

    public float spotAngle;

    public Spotlight(){}

}
