using UnityEngine;

public class CubeHole : Hole
{
    public override bool ValidateShape(Shape shape)
    {
        return shape is Cube;
    }

    protected override float[] GetValidAnglesForShape(Shape shape)
    {
        return new float[] { 0f, 90f, 180f, 270f };
    }
}