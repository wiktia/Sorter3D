using UnityEngine;

public class Xylophone : MonoBehaviour
{
    public AudioClip[] noteSounds;
    private AudioSource audioSource;
    private MeshCollider meshCollider;
    public Level2 level2;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Brak komponentu AudioSource na obiekcie ksylofonu!");
        }
        else
        {
            Debug.Log("Komponent AudioSource znaleziony!");
        }

        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            Debug.LogError("Brak komponentu MeshCollider na obiekcie ksylofonu!");
        }
        else
        {
            Debug.Log("Komponent MeshCollider znaleziony!");
        }

        if (noteSounds == null || noteSounds.Length == 0)
        {
            Debug.LogError("Brak dźwięków w tablicy noteSounds!");
        }
        else
        {
            Debug.Log("Znaleziono " + noteSounds.Length + " dźwięków w tablicy noteSounds");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Kolizja z: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag + ", Layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.CompareTag("Mallet"))
        {
            Debug.Log("Ksylofon uderzony przez pałkę!");
            OnHit(collision.GetContact(0).point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coś weszło w trigger: " + other.gameObject.name);

        if (other.CompareTag("Mallet"))
        {
            Debug.Log("Pałka dotknęła ksylofonu!");
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            OnHit(hitPoint);
        }
    }

    public void OnHit(Vector3 hitPoint)
    {
        Debug.Log($"Punkt uderzenia (globalny): {hitPoint}");
        int noteIndex = MapHitPointToNote(hitPoint);
        Debug.Log($"Indeks nuty do odtworzenia: {noteIndex}");
        PlayNote(noteIndex);
    }

    private int MapHitPointToNote(Vector3 hitPoint)
    {
        if (meshCollider == null || noteSounds.Length == 0)
            return 0;

        // przeliczenie punktu uderzenia na lokalny układ współrzędnych
        Vector3 localHitPoint = transform.InverseTransformPoint(hitPoint);
        Bounds bounds = meshCollider.sharedMesh.bounds; // Lokalny układ współrzędnych

        // obliczenie procentowego przesunięcia
        float hitPercent = (localHitPoint.x - bounds.min.x) / bounds.size.x;

        // zakres procentowy (od 0 do 1)
        float minPercent = 0f;
        float maxPercent = 1f;

        // obliczenie kroku dla 6 nut
        float step = (maxPercent - minPercent) / noteSounds.Length;

        // mapowanie punktu uderzenia do przedziału nut
        int noteIndex = Mathf.FloorToInt((hitPercent - minPercent) / step);

        // ograniczenie wartości do zakresu dostępnych nut
        noteIndex = Mathf.Clamp(noteIndex, 0, noteSounds.Length - 1);

        Debug.Log($"Procent uderzenia: {hitPercent:F3}, Indeks nuty: {noteIndex}");
        return noteIndex;
    }

    public void PlayNote(int noteIndex)
{
    if (audioSource != null && noteIndex >= 0 && noteIndex < noteSounds.Length)
    {
        Debug.Log("Odtwarzam dźwięk o indeksie: " + noteIndex);
        audioSource.PlayOneShot(noteSounds[noteIndex]);

        // powiadomienie Level2 o zagranej nucie
        level2?.PlayNoteAndCheckSequence(noteIndex);
    }
    else
    {
        Debug.LogWarning("Nie można odtworzyć dźwięku. audioSource jest null lub indeks nuty jest poza zakresem");
    }
}
}
