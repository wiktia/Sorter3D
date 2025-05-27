using UnityEngine;

public class Cube : Shape
{
    
    protected override void OnPickUp()
    {
        Debug.Log("szczescian podniesiony");
    }

   
    protected override void OnDrop()
    {
        Debug.Log("szczescian upuszczony");
    }

   
    public override void OnInteract()
    {
        
    }
}
