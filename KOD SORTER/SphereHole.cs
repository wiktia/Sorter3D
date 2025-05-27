using UnityEngine;

public class SphereHole : Hole
{
    public override bool ValidateShape(Shape shape)
    {
        return shape is Sphere;
    }

    protected override float[] GetValidAnglesForShape(Shape shape)
    {
        return new float[] { 0f, -180f }; 
    }
}
