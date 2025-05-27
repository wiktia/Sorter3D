using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f; // czulosc myszki
    public Transform playerBody;         // cialo gracza (capsule)

    private float xRotation = 0f;     

    void Start()
    {
        //ukrycie kursora
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // pobieranie ruchu myszy
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // obrót w osi X (góra-dół)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Ograniczenie kąta widzenia

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // obrót gracza w osi Y (lewo-prawo)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

