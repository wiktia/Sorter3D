using UnityEngine;

public class CylinderHole : Hole
{
    public override bool ValidateShape(Shape shape)  
    {
        return shape is Cylinder;
    }

    protected override float[] GetValidAnglesForShape(Shape shape)
    {
        return new float[] { 0f, 180f }; // poprawne kąty dla walca, przekazywane do hole
    }
}
