using UnityEngine;
using System.Collections;

public class Level2 : Level
{
    public GameObject[] cylinderFragments;
    private int fragmentsCollected = 0;
    public GameObject cylinderShapePrefab;
    public ObjectPickup objectPickup;

    private int placedObjects = 0;
    public int totalObjectsToPlace = 4;

    public AudioClip[] noteSounds;
    public GameObject starFigurePrefab;
    public Transform handTransform;
    public Transform playerTransform;
    public float riseSpeed = 2f;
    private Vector3 originalPlayerPosition;
    

    private int[] correctSequence = { 0, 0, 0, 0, 4, 4, 4, 4, 5, 5, 5, 5, 4, 4 };//sekwencja(z jakiegos powodu wszystkie x2)
    private int[] playerSequence;
    private int currentSequenceIndex = 0;

    private void OnEnable()
    {
        Hole.OnShapePlaced += HandleShapePlaced;
    }

    private void OnDisable()
    {
        Hole.OnShapePlaced -= HandleShapePlaced;
        if (playerTransform != null)
        {
            playerTransform.position = originalPlayerPosition;
        }
    }

    protected override void Start()
    {   base.Start();
        foreach (GameObject fragment in cylinderFragments)
        {
            fragment.AddComponent<Fragment>().level2 = this;
        }

        playerSequence = new int[correctSequence.Length];
        originalPlayerPosition = playerTransform.position;
    }

    public void FragmentCollected(GameObject fragment)
    {
        fragmentsCollected++;
        Destroy(fragment);
        Debug.Log("Zebrano fragmentów: " + fragmentsCollected + "/" + cylinderFragments.Length);

        if (fragmentsCollected >= cylinderFragments.Length)
        {
            Debug.Log("Wszystkie fragmenty zebrane! Tworzę walec.");
            CreateCylinder();
        }
    }

    private void CreateCylinder()
    {
        if (objectPickup.holdPoint == null)
        {
            Debug.LogError("objectPickup.holdPoint jest null!");
            return;
        }

        GameObject currentCylinderInstance = Instantiate(cylinderShapePrefab, objectPickup.holdPoint.position, objectPickup.holdPoint.rotation);

        if (currentCylinderInstance == null)
        {
            Debug.LogError("Instancja jest null!");
            return;
        }

        objectPickup.PickUpObject(currentCylinderInstance);

        Debug.Log("Walec utworzony i umieszczony w dłoni.");
    }

    private void HandleShapePlaced()
    {
        placedObjects++;
        Debug.Log("Obiekty umieszczone: " + placedObjects + "/" + totalObjectsToPlace);
        if (placedObjects >= totalObjectsToPlace)
        {
            Debug.Log("Level 2 ukończony!");
            LevelComplete();
        }
    }

    public void PlayNoteAndCheckSequence(int noteIndex)
    {
        if (noteIndex == correctSequence[currentSequenceIndex])
        {
            playerSequence[currentSequenceIndex] = noteIndex;
            currentSequenceIndex++;
            Debug.Log("Poprawna nuta!");

            if (currentSequenceIndex >= correctSequence.Length)
            {
                Debug.Log("Sekwencja zakończona poprawnie!");

                // niszczymy palke po dobrej sekwencji
                if (objectPickup != null && objectPickup.heldObject != null && objectPickup.heldObject.CompareTag("Mallet"))
                {
                    Debug.Log("Niszczę pałkę: " + objectPickup.heldObject.name);
                    Destroy(objectPickup.heldObject);
                    objectPickup.heldObject = null;
                }
                else
                {
                    GameObject malletInScene = GameObject.FindGameObjectWithTag("Mallet");
                    if (malletInScene != null)
                    {
                        Debug.Log("Znaleziono pałkę w scenie i niszczę: " + malletInScene.name);
                        Destroy(malletInScene);
                    }
                    else
                    {
                        Debug.Log("Nie znaleziono pałki w scenie.");
                    }
                }

                GrantReward();
                ResetSequence();
            }
        }
        else
        {
            Debug.LogWarning("Błędna nuta! Resetowanie sekwencji.");
            ResetSequence();
        }
    }

    private void ResetSequence()
    {
        currentSequenceIndex = 0;
        playerSequence = new int[correctSequence.Length];
    }

    private void GrantReward()
    {
        Debug.Log("GrantReward wywołane!");

        if (playerTransform == null)
        {
            Debug.LogError("playerTransform jest NULL w GrantReward()!");
            return;
        }
        else
        {
            Debug.Log("playerTransform jest OK w GrantReward(): " + playerTransform.name);
        }

        Debug.Log("Przyznaję gwiazdkę po 3 sekundach.");

        StartCoroutine(GiveStarWithDelay(3f)); // przekazujemy 3 sekundy do korutyny
    }

    private IEnumerator GiveStarWithDelay(float delay)
    {
        Debug.Log("GiveStarWithDelay() rozpoczęte! Czekam " + delay + " sekund.");

        yield return new WaitForSeconds(delay); // czekamy 3s

        if (starFigurePrefab == null)
            Debug.LogError("starFigurePrefab jest NULL!");
        else
            Debug.Log("starFigurePrefab jest OK: " + starFigurePrefab.name);

        if (handTransform == null)
            Debug.LogError("handTransform jest NULL!");
        else
            Debug.Log("handTransform jest OK: " + handTransform.name);

        if (objectPickup == null)
            Debug.LogError("objectPickup jest NULL!");
        else
            Debug.Log("objectPickup jest OK: " + objectPickup.name);

        if (starFigurePrefab != null && handTransform != null && objectPickup != null)
        {
            GameObject star = Instantiate(starFigurePrefab, handTransform.position, handTransform.rotation);
            Debug.Log("Utworzono gwiazdę: " + star.name);

            if (objectPickup.heldObject != null)
            {
                Debug.Log("ObjectPickup trzyma obiekt. Przypisuję gwiazdę do trzymanego obiektu.");
                star.transform.SetParent(objectPickup.heldObject.transform);
                star.transform.localPosition = Vector3.zero;
                star.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.Log("ObjectPickup NIE trzyma obiektu. Przypisuję gwiazdę do handTransform.");
                star.transform.SetParent(handTransform);
                star.transform.localPosition = Vector3.zero;
                star.transform.localRotation = Quaternion.identity;
            }

            objectPickup.PickUpObject(star);
            Debug.Log("Gwiazda przekazana do ObjectPickup");
            Debug.Log("Skala gwiazdy po wręczeniu: " + star.transform.localScale);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego prefabrykata figury gwiazdy lub miejsca do jej spawnowania.");
        }
    }

     public override void LevelComplete()
    {
        base.LevelComplete();
        Debug.Log("Level 2 zakończony.");
    }
}