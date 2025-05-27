using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1 : Level
{
    private int placedObjects = 0;
    public int totalObjectsToPlace = 4;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Level 1 rozpoczęty.");
    }

    public override void LevelComplete()
    {
        base.LevelComplete();
        Debug.Log("Level 1 zakończony.");
    }

    private void OnEnable()
    {
        Hole.OnShapePlaced += HandleShapePlaced; //tutaj obserwujemy czy zaszlo zdarzenie on shape placed obserwator
    }

    private void OnDisable()
    {
        Hole.OnShapePlaced -= HandleShapePlaced;
    }

    private void HandleShapePlaced() //tutaj zbieramy informacje czy zaszlo on shape placed, a jesli zaszlo dodajemy obiekt do umieszczonych obiektow. kiedy umieszczone obiekty= wszystkie obiekty w levelu, nastepny poziom
    {
        placedObjects++;
        Debug.Log("Obiekty umieszczone: " + placedObjects + "/" + totalObjectsToPlace); 
        if (placedObjects >= totalObjectsToPlace)
        {
            Debug.Log("Level 1 ukończony!"); 
            LevelComplete();
        }
    }
}