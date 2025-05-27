using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//klasa kontrolujaca zachowania gracza specyficzne dla levelu3

public class PlayerController2 : MonoBehaviour
{
    
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundMask; 
    public LayerMask wallMask; 
    private Level3 level3;
    public SceneFader sceneFader;

    public float maxFarClipPlane = 10000f; // zasięg widzenia kamery po zniszczeniu wszystkich scian

    public Vector3 minBounds = new Vector3(-150, 1, -150); 
    public Vector3 maxBounds = new Vector3(150, 10, 150); 

    public int totalWalls = 4; // całkowita liczba ścian do zniszczenia
    public Transform targetCameraPosition; //
    public float zoomSpeed = 5f; 

    public LevelManager levelManager;
    private bool isGameOver = false;

    
    private Rigidbody rb;
    public Camera playerCamera;
    private float rotationX = 0f;
    private bool isGrounded;
    private int destroyedWalls = 0;
    private float timerAfterWallsDestroyed = 0f;
    private bool allWallsDestroyed = false;
    private bool playerAtTargetPosition = false;

    public AudioSource cameraAudioSource; 
    public AudioClip zoomOutSound; //dzwiek dla oddalania kamery
    private void Start()
    {
        level3 = FindObjectOfType<Level3>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        // ukrycie kursora
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // obrót kamery (w poziomie i pionie)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        }
        else
        {
            Debug.LogError("playerCamera jest null! Upewnij się, że kamera gracza jest przypisana w Inspektorze!", this);
        }

        //sprawdzanie odleglosci od gruntu i skok
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // ruch gracza (wraz z ograniczeniem do granic mapy i wykrywaniem kolizji ze ścianami)
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 movement = transform.TransformDirection(new Vector3(xDirection, 0, zDirection).normalized * speed * Time.deltaTime);

        if (!IsWallBlocking(movement))
        {
            Vector3 newPosition = transform.position + movement;
            float clampedX = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            float clampedZ = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
        }

        // **LOGIKA PO ZNISZCZENIU WSZYSTKICH ŚCIAN**
        if (destroyedWalls == totalWalls && !allWallsDestroyed)
        {
            Debug.Log("Wszystkie ściany zniszczone!");
            allWallsDestroyed = true;

            // **USUWANIE OBIEKTÓW I ZMIANA ZASIĘGU WIDZENIA KAMERY (WYKONYWANE RAZ)**
            GameObject[] ceilingObjects = GameObject.FindGameObjectsWithTag("Ceiling");
            foreach (GameObject ceilingObject in ceilingObjects) { Destroy(ceilingObject); }
            GameObject[] skyObjects = GameObject.FindGameObjectsWithTag("Sky");
            foreach (GameObject skyObject in skyObjects) { Destroy(skyObject); }
            playerCamera.farClipPlane = maxFarClipPlane;
        }

        
        if (allWallsDestroyed && !playerAtTargetPosition)
        {
            timerAfterWallsDestroyed += Time.deltaTime;
            Debug.Log($"Timer po zniszczeniu ścian: {timerAfterWallsDestroyed}");

            if (timerAfterWallsDestroyed >= 20f)
            {
                Debug.Log("Minęło 20 sekund! Przenoszę kamerę do pozycji docelowej.");

                // przesuwanie kamery w kierunku docelowej pozycji
                StartCoroutine(MoveCameraToTargetPosition());

                playerAtTargetPosition = true; // Ustawienie flagi po przeniesieniu
                timerAfterWallsDestroyed = 0f; // Reset timera
            }
        }

        // gdy kamera jest na docelowej pozycji po odliczeniu zmiana sceny
        if (playerAtTargetPosition)
        {
            Debug.Log("Kamera jest na pozycji docelowej! Odliczam do zmiany sceny.");
            rb.velocity = Vector3.zero; 
            rb.isKinematic = true; 
            enabled = false; // wyłączenie PlayerController2 (uniemożliwienie dalszego ruchu)
            StartCoroutine(WaitBeforeEndScreen());
            playerAtTargetPosition = false; // reset flagi, aby kod wykonał się tylko raz
        }
    }

    private IEnumerator MoveCameraToTargetPosition()
    {
         float distance = Vector3.Distance(playerCamera.transform.position, targetCameraPosition.position);
        float moveSpeed = speed * 3f; 

        if (cameraAudioSource != null && zoomOutSound != null)

        {   
            cameraAudioSource.PlayOneShot(zoomOutSound);
            cameraAudioSource.volume = 0.4f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        while (distance > 0.1f)
        {
            playerCamera.transform.position = Vector3.MoveTowards(playerCamera.transform.position, targetCameraPosition.position, Time.deltaTime * speed);
            distance = Vector3.Distance(playerCamera.transform.position, targetCameraPosition.position);
            yield return null;
        }
        playerCamera.transform.position = targetCameraPosition.position;
    }

    private IEnumerator WaitBeforeEndScreen()
    {
        yield return new WaitForSeconds(30f);
        
        levelManager.LoadEndScreen();
    }

    private bool IsWallBlocking(Vector3 movement)
    {
        return Physics.Raycast(transform.position, movement.normalized, movement.magnitude, wallMask);
    }

    public void IncrementDestroyedWalls()
    {
        destroyedWalls++;
        Debug.Log($"Zniszczono ścianę. destroyedWalls: {destroyedWalls}");
    }
}
