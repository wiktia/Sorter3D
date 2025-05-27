using UnityEngine;

public class Sphere : Shape
{
    protected override void OnPickUp()
    {
        Debug.Log("Kula podniesiona");
    }

    protected override void OnDrop()
    {
        Debug.Log("kula upuszczona");
    }

    public override void OnInteract()
    {
        
    }
}