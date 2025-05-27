using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public Transform holdPoint;
    public GameObject heldObject;
    private Rigidbody heldRigidbody;
    private Collider heldObjectCollider;

    public void PickUpObject(GameObject obj)
    {
        if (heldObject == null)
        {
            heldObject = obj;
            heldRigidbody = heldObject.GetComponent<Rigidbody>();
            heldObjectCollider = heldObject.GetComponent<Collider>();

            if (heldRigidbody != null)
            {
                heldRigidbody.isKinematic = true;
                heldRigidbody.useGravity = false;
            }
            if (heldObjectCollider != null)
            {
                heldObjectCollider.enabled = false;
            }

            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.Euler(180,0, 0); // Obrót pałki
        }
    }

    public void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);

            if (heldRigidbody != null)
            {
                heldRigidbody.isKinematic = false;
                heldRigidbody.useGravity = true;
                heldRigidbody = null; 
            }
            if(heldObjectCollider != null)
            {
                heldObjectCollider.enabled = true;
                heldObjectCollider = null;
            }

            heldObject = null; 
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && heldObject != null)
        {
            DropObject();
        }
        if (Input.GetMouseButtonUp(1) && heldObject != null)
        {
            DropObject();
        }
    }
}