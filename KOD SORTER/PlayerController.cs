using UnityEngine;

public class PlayerController : MonoBehaviour
{
public float speed = 5f;
public float mouseSensitivity = 2f;
public float verticalLookLimit = 80f;
public float jumpForce = 5f;
public float gravityMultiplier = 2f; 
public float groundCheckDistance = 0.1f;
public LayerMask groundMask;

private Rigidbody rb;
private Camera playerCamera;
private float rotationX = 0f;
private bool isGrounded;

public Vector3 minBounds = new Vector3(-150, 1, -150);
public Vector3 maxBounds = new Vector3(150, 10, 150);

public LayerMask collisionLayer; // warstwa kolizji to warstwa wall

private void OnCollisionEnter(Collision collision)
{
if ((1 << collision.gameObject.layer & collisionLayer) != 0)
{
// Kolizja ze ścianą
Debug.Log("Kolizja ze ścianą!");
Vector3 direction = transform.position - collision.contacts[0].point;
direction.y = 0; 
rb.velocity = Vector3.ProjectOnPlane(rb.velocity, direction);
}
}

void Start()
{
rb = GetComponent<Rigidbody>();
playerCamera = Camera.main;
rb.freezeRotation = true; 

Cursor.lockState = CursorLockMode.Locked;
Cursor.visible = false;
}

void Update()
{
// ruch kamery (obroty)
float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
transform.Rotate(Vector3.up * mouseX);

float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
rotationX -= mouseY;
rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);
playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

// ruch gracza
float xDirection = Input.GetAxis("Horizontal");
float zDirection = Input.GetAxis("Vertical");

Vector3 movement = transform.TransformDirection(new Vector3(xDirection, 0, zDirection).normalized * speed * Time.deltaTime);

Vector3 newPosition = transform.position + movement;

float clampedX = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
float clampedZ = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

// sprawdzanie gruntu i skakanie
isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

if (Input.GetButtonDown("Jump") && isGrounded)
{
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
}

// gdy gracz nie jest na ziemii(czyli skacze)
if (!isGrounded)
{
rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
}
}

}