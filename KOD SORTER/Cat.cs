using UnityEngine;

public class Cat : Shape
{
    public GameObject cubePrefab;
    private Rigidbody catRigidbody;
    private Collider catCollider;

    private void Start()
    {
        catRigidbody = GetComponent<Rigidbody>();
        catCollider = GetComponent<Collider>();
    }

    protected override void OnPickUp()
    {
        base.OnPickUp();

        if (catRigidbody != null)
        {
            catRigidbody.isKinematic = true;
            catRigidbody.useGravity = false;
        }

        if (catCollider != null)
        {
            catCollider.enabled = false;
        }

        transform.SetParent(PlayerInteraction.Instance.holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 180, 0);

        isPlaced = false;
    }

    protected override void OnDrop()
    {
        base.OnDrop();

        if (catRigidbody != null)
        {
            catRigidbody.isKinematic = false;
            catRigidbody.useGravity = true;
        }

        if (catCollider != null)
        {
            catCollider.enabled = true;
        }

        isPlaced = true;
    }

    public void ThrowCat(Vector3 throwDirection, float throwForce)
    {
        if (!isPlaced)
        {
            GameObject cube = Instantiate(cubePrefab, transform.position, transform.rotation);
            Destroy(gameObject); // po rzucie kotem usuwam go i zamieniam w szczescian
            
            cube.AddComponent<Shape>();

            if (cube.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
                rb.drag = 0.5f;
            }
        }
    }
    
}