using UnityEngine;
using System.Collections;

public class CatSpawner : MonoBehaviour
{
    public GameObject catPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 60f; 

    private void Start()
    {
        StartCoroutine(SpawnCatAfterDelay());
    }

    private IEnumerator SpawnCatAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (catPrefab != null && spawnPoint != null)
        {
            Instantiate(catPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Nie ustawiono prefabu kota lub punktu spawnu w CatSpawner!");
        }
    }
}