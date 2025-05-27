using UnityEngine;

public class Star : Shape
{ 
    
    protected override void OnPickUp()
    {
        Debug.Log("Gwiazda podniesiona");
    }

   
    protected override void OnDrop()
    {
        Debug.Log("Gwiazdka upuszczona");
    }

    
    public override void OnInteract()
    {
        
    }
}
