using UnityEngine;



public class Cylinder : Shape
{
    
    protected override void OnPickUp()
    {
        Debug.Log("walec podniesiony");
    }

    
    protected override void OnDrop()
    {
        Debug.Log("walec upuszczony");
    }

   
    public override void OnInteract()
    {
        
    }
}
