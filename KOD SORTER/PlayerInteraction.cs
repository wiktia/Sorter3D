using System.Collections;
using UnityEngine;
using System;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance { get; private set; }
    public float interactionRadius = 2f;
    public Transform holdPoint;
    public float throwForce = 1000f;
    public float throwForwardMultiplier = 2f;
    public Collider playerCollider;
    public float rotationSpeed = 100f;
    public float snapAngleThreshold = 10f;
    public float snapDistanceThreshold = 100f;
    public float minSnapSpeed = 2f;
    public float maxSnapSpeed = 15f;
    public AnimationCurve snapSpeedCurve;
    

    private Shape currentShape;
    private Hole currentHole;
    private Quaternion targetRotation;
    private bool isSnapping = false;
    private bool isAligned = false;

    private Camera playerCamera;
    public static event Action<Shape> OnShapePickedUp;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerCamera = Camera.main; 
    }

    void Update()
    {
        if (currentShape != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isAligned && currentHole != null)
                {
                    SnapToHole();
                }
                else
                {
                    ThrowShape();
                }
            }

            if (!isSnapping)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    currentShape.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
                    checkForHole();
                }
                if (Input.GetKey(KeyCode.R))
                {
                    currentShape.transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime, Space.World);
                    checkForHole();
                }
            }
            else
            {
                SnapWithCurve();
            }
        }
    }

    public void TryPickUp(GameObject obj)
    {
        if (currentShape == null)
        {
            if(obj.TryGetComponent<Shape>(out Shape shape))
            {
                currentShape = shape;
                currentShape.PickUp(holdPoint);
                OnShapePickedUp?.Invoke(currentShape);

                 if (currentShape.TryGetComponent(out Cat cat))
                {
                    cat.PickUp(transform);
                }
            }
        }
    }

    private void checkForHole() // odbiorca 
    {
        Vector3 throwDirection = transform.forward;
        if (Physics.Raycast(transform.position, throwDirection, out RaycastHit hit, 100f))
        {
            Hole hole = hit.collider.GetComponent<Hole>();
            if (hole != null)
            {
                currentHole = hole;
                targetRotation = CalculateTargetRotation(hole);

                //poprawnosc rotacji jest tu sprawdzana dzieki klasie hole
                float angleDifference = Quaternion.Angle(currentShape.transform.rotation, targetRotation);
                float distanceToHole = Vector3.Distance(currentShape.transform.position, hole.transform.position);
                
                // jak roznica tych katow jest <10 to pasuje
                if (angleDifference <= snapAngleThreshold && distanceToHole < snapDistanceThreshold)
                {
                    isSnapping = true;

                   
                    if (angleDifference < 10f)
                    {
                        currentShape.transform.rotation = targetRotation;
                        isAligned = true;                                             //jesli rotacja git i figura git snap
                    }
                }
                else
                {
                    isSnapping = false;
                    isAligned = false;
                }
            }
            else
            {
                currentHole = null;
                isSnapping = false;
                isAligned = false;
            }
        }
        else
        {
            currentHole = null;
            isSnapping = false;
            isAligned = false;
        }
    }

    private Quaternion CalculateTargetRotation(Hole hole)
    {
        Quaternion targetRotation = Quaternion.LookRotation(-hole.transform.forward);
        targetRotation *= Quaternion.Euler(0, hole.transform.eulerAngles.y, 0);
        return targetRotation;
    }

    private void SnapToHole()
    {
        if (currentShape != null && currentHole != null)
        {
            currentShape.transform.position = currentHole.transform.position;
            currentShape.transform.rotation = CalculateTargetRotation(currentHole);

            if (currentShape.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true; 
                rb.useGravity = false;
            }

            Collider collider = currentShape.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false; 
            }

            currentShape = null;
            currentHole = null;
            isAligned = false;
        }
    }

    private void SnapWithCurve()
    {
        if (currentShape != null && currentHole != null)
        {
            float distanceToHole = Vector3.Distance(currentShape.transform.position, currentHole.transform.position);
            float normalizedDistance = Mathf.Clamp01(1f - (distanceToHole / snapDistanceThreshold));

            float snapSpeed = snapSpeedCurve.Evaluate(normalizedDistance) * (maxSnapSpeed - minSnapSpeed) + minSnapSpeed;

            currentShape.transform.rotation = Quaternion.Slerp(currentShape.transform.rotation, targetRotation, Time.deltaTime * snapSpeed);
            currentShape.transform.position = Vector3.Lerp(currentShape.transform.position, currentHole.transform.position, Time.deltaTime * snapSpeed);

            if (Quaternion.Angle(currentShape.transform.rotation, targetRotation) < 1f && distanceToHole < 0.05f)
            {
                isSnapping = false;
                currentShape.transform.rotation = targetRotation;
                currentShape.transform.position = currentHole.transform.position;
                isAligned = true;

                if (currentShape.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                Collider collider = currentShape.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
                currentShape = null;
                currentHole = null;
            }
        }
    }

    private void ThrowShape()
{
    if (currentShape != null)
    {
        float cameraPitch = playerCamera.transform.eulerAngles.x;
        if (cameraPitch > 180) cameraPitch -= 360;
        float verticalThrowMultiplier = Mathf.Clamp(cameraPitch / 90f, 0.4f, -0.4f);

        // przy rzucie uzwgledniamy pozycje gracza
        Vector3 throwDirection = transform.forward;
        throwDirection += transform.up * verticalThrowMultiplier;

        if (currentShape.TryGetComponent<Cat>(out Cat cat))
        {
            cat.ThrowCat(throwDirection, throwForce);
            currentShape = null;
            currentHole = null;
            isAligned = false;
            return; 
        }

        currentShape.Drop(transform.position + throwDirection); 
        if (currentShape.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            rb.drag = 0.5f;

            if (playerCollider != null)
            {
                StartCoroutine(EnableCollisionAfterDelay(currentShape.GetComponent<Collider>(), playerCollider, 0.1f));
            }
        }

        currentShape = null;
        currentHole = null;
        isAligned = false;
    }
}

    private System.Collections.IEnumerator EnableCollisionAfterDelay(Collider objCollider, Collider playerCollider, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics.IgnoreCollision(objCollider, playerCollider, false);
    }
}
