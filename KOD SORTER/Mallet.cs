using UnityEngine;

public class Mallet : MonoBehaviour
{
    private ObjectPickup objectPickup;
    private GameObject player;
    public float hitAngle = 45f;
    public float hitSpeed = 10f;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private bool isHitting = false;
    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        Xylophone xylophone = other.GetComponent<Xylophone>();
        if (xylophone != null)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            xylophone.OnHit(hitPoint);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            objectPickup = player.GetComponent<ObjectPickup>();
            if (objectPickup == null)
            {
                Debug.LogError("Obiekt gracza nie ma komponentu ObjectPickup!");
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu z tagiem 'Player'!");
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Pałka nie ma komponentu Rigidbody!");
        }
        originalRotation = transform.localRotation; 
    }

    private void OnMouseDown()
    {
        if (player != null && objectPickup != null && objectPickup.heldObject == null)
        {
            objectPickup.PickUpObject(this.gameObject);
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    private void Update()
    {
        if (objectPickup != null && objectPickup.heldObject != this.gameObject && rb != null)
        {
            rb.isKinematic = false;
        }

        if (objectPickup != null && objectPickup.heldObject == this.gameObject)
        {
            if (Input.GetMouseButtonDown(0) && !isHitting)
            {
                originalRotation = transform.localRotation;
                targetRotation = originalRotation * Quaternion.Euler(hitAngle, 0, 0);
                isHitting = true;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
                {
                    Xylophone xylophone = hit.collider.GetComponent<Xylophone>();
                    if (xylophone != null)
                    {
                        xylophone.OnHit(hit.point);
                    }
                }
            }
        }

        if (isHitting)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * hitSpeed);
            if (Quaternion.Angle(transform.localRotation, targetRotation) < 1f)
            {
                transform.localRotation = originalRotation;
                isHitting = false;
            }

            Physics.SyncTransforms();
        }
    }

    public void ResetMallet()
    {
        transform.localRotation = originalRotation;
        isHitting = false;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}