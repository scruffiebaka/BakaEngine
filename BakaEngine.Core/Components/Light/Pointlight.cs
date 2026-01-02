using System;
using OpenTK.Mathematics;

namespace BakaEngine.Core.Components;

public class Pointlight : Light
{
    public Vector3 position = Vector3.Zero;
    public Pointlight(){}
}
