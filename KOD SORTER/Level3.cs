using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3 : Level
{
    private int placedObjects = 0;
    public int totalObjectsToPlace = 4;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Level 3 rozpoczęty.");
    }

    public override void LevelComplete()
    {
        base.LevelComplete();
        Debug.Log("Level 3 zakończony.");
    }

    private void OnEnable()
    {
        Hole.OnShapePlaced += HandleShapePlaced;
    }

    private void OnDisable()
    {
        Hole.OnShapePlaced -= HandleShapePlaced;
    }

    private void HandleShapePlaced()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Wall")) 
    {
        
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        
        Vector3 forceDirection = -collision.contacts[0].normal;
        float forceMagnitude = 100f; 
        GetComponent<Rigidbody>().AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
    }
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hole"))
        {
            
        }
    }
}