using UnityEngine;

public class StarHole : Hole
{
    public override bool ValidateShape(Shape shape)
    {
        return shape is Star;
    }

    protected override float[] GetValidAnglesForShape(Shape shape)
    {
        return new float[] { 0f, 180f }; // poprawne kąty dla gwiazdki
    }
}
