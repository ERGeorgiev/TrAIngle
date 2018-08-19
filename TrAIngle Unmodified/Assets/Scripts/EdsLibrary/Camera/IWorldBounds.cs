using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldBounds
{
    Vector3 WorldPosition(float z = 0);
    Vector3 WorldBoundsSize();
}

public interface IWorldBounds2D
{
    Vector3 WorldPosition(float z = 0);
    Vector2 WorldBoundsSizeVector();
}