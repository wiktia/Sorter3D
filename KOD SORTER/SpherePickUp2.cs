using UnityEngine;

public class SpherePickupHandler : MonoBehaviour
{
    public GameObject imageToDestroy; 

    private void OnEnable()
    {
        PlayerInteraction.OnShapePickedUp += HandleSpherePickup; // Subskrybujemy event
    }

    private void OnDisable()
    {
        PlayerInteraction.OnShapePickedUp -= HandleSpherePickup; // Odsubskrybujemy event
    }

    private void HandleSpherePickup(Shape pickedUpShape)
    {
        if (pickedUpShape.gameObject == this.gameObject) // Sprawdzamy, czy podniesiony obiekt to ta kula
        {
            Debug.Log("Kula została podniesiona!");
            if (imageToDestroy != null)
            {
                Destroy(imageToDestroy);
                Debug.Log("Obraz został zniszczony.");
            }
           
        }
    }
}