using UnityEngine;

public class Level2Crouch : MonoBehaviour
{
    private Vector3 originalScale;
    public float crouchScaleY = 0.5f;
    public float crouchSpeed = 5f;
    private ObjectPickup objectPickup;
    private Transform playerTransform;
    public string malletTag = "Mallet";

    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private void Start()
    {
        playerTransform = transform;
        originalScale = playerTransform.localScale;
        objectPickup = GetComponent<ObjectPickup>();
        if (objectPickup == null)
        {
            Debug.LogError("Obiekt gracza nie ma komponentu ObjectPickup!");
        }
    }

    private void Update()
    {
        if (objectPickup != null && objectPickup.heldObject != null && objectPickup.heldObject.CompareTag(malletTag))
        {
            originalParent = objectPickup.heldObject.transform.parent;
            originalLocalPosition = objectPickup.heldObject.transform.localPosition;
            originalLocalRotation = objectPickup.heldObject.transform.localRotation;
            objectPickup.heldObject.transform.SetParent(null);

            Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * crouchScaleY, originalScale.z);
            playerTransform.localScale = Vector3.Lerp(playerTransform.localScale, targetScale, Time.deltaTime * crouchSpeed);

            objectPickup.heldObject.transform.SetParent(originalParent);
            objectPickup.heldObject.transform.localPosition = originalLocalPosition;
            objectPickup.heldObject.transform.localRotation = originalLocalRotation;
        }
        else if (playerTransform.localScale != originalScale)
        {
            if (objectPickup.heldObject != null)
            {
                originalParent = objectPickup.heldObject.transform.parent;
                originalLocalPosition = objectPickup.heldObject.transform.localPosition;
                originalLocalRotation = objectPickup.heldObject.transform.localRotation;
                objectPickup.heldObject.transform.SetParent(null);
            }

            playerTransform.localScale = Vector3.Lerp(playerTransform.localScale, originalScale, Time.deltaTime * crouchSpeed);

            if (objectPickup.heldObject != null)
            {
                objectPickup.heldObject.transform.SetParent(originalParent);
                objectPickup.heldObject.transform.localPosition = originalLocalPosition;
                objectPickup.heldObject.transform.localRotation = originalLocalRotation;
            }
        }
    }
}