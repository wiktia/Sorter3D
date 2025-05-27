using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public bool isPlaced = false;
    public enum ShapeType { Cube, Sphere, Cylinder, Star }
    public ShapeType shapeType;
    public bool IsPickedUp { get; private set; } = false;

    public void PickUp(Transform parent)
    {
        IsPickedUp = true;

        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 1, 0);

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        OnPickUp();
    }

    public void Drop(Vector3 dropPosition)
    {
        IsPickedUp = false;

        transform.SetParent(null);
        transform.position = dropPosition;

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
        }

        OnDrop();
    }

  private void OnMouseDown()
{
    if (!IsPickedUp && PlayerInteraction.Instance != null)
    {
        PlayerInteraction.Instance.TryPickUp(gameObject);
    }
    else
    {
        
    }
}
    protected virtual void OnPickUp()
    {
        Debug.Log($"[Shape] {gameObject.name} został podniesiony.");
    }

    protected virtual void OnDrop()
    {
        Debug.Log($"[Shape] {gameObject.name} został upuszczony.");
    }

    public virtual void OnInteract()
    {
        Debug.Log($"[Shape] {gameObject.name} został użyty.");
    }
}