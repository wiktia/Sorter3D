using UnityEngine;
//obserwator, strategia
public abstract class Hole : MonoBehaviour
{
    private const float Tolerance = 10f;

    public delegate void ShapePlaced();
    public static event ShapePlaced OnShapePlaced; //EVENT ON SHAPE PLACED dla wzorca obserwator

    public AudioClip placementSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Shape shape = other.GetComponent<Shape>();
        if (shape == null || !ValidateShape(shape) || shape.isPlaced) return;

        // kula- rotacja jest zawsze poprawna
        if (shape is Sphere)
        {
            Debug.Log("Kula - rotacja zawsze poprawna!");
        }
        // dla innych kształtów
        else if (!IsRotationValid(other.transform.rotation, transform.rotation, GetValidAnglesForShape(shape), shape))
        {
            Debug.LogWarning($"Kształt {shape.shapeType} ma nieprawidłową rotację!");
            return;
        }

       
        other.transform.position = transform.position;
        other.transform.rotation = transform.rotation;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider collider = other.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        shape.isPlaced = true;

        // WYWOLANIE EVENTU
        OnShapePlaced?.Invoke();

        // odtwarzanie dźwięku
        if (placementSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(placementSound);   //gdy wywolanu event on shape placed dzwiek
        }

        Debug.Log("Kształt " + shape.shapeType + " pasuje do otworu!");
    }

    // metoda walidująca kształt
    public virtual bool ValidateShape(Shape shape)
    {
        return false; //domyslnie false
    }

    
    protected bool IsRotationValid(Quaternion objectRotation, Quaternion targetRotation, float[] validAngles, Shape shape)
    {
        
        if (shape is Sphere)
        {
            return true; // kula ma dowolną rotację
        }

        //rotacja dla reszty
        float objectY = objectRotation.eulerAngles.y;
        float targetY = targetRotation.eulerAngles.y;

        foreach (float validAngle in validAngles) //tu liczy sie czy rotacja jest poprawne
        {
            float difference = Mathf.Abs(Mathf.DeltaAngle(objectY, targetY + validAngle));
            if (difference <= Tolerance)
            {
                return true;
            }
        }

        return false;
    }

    // metoda abstrakcyjna, która zwraca poprawne kąty dla danego otworu 
    protected abstract float[] GetValidAnglesForShape(Shape shape);
}
