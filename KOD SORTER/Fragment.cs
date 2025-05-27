using UnityEngine;


public class Fragment : MonoBehaviour
{
    public Level2 level2; // referencja do Level2

    private void OnMouseDown()
    {
        level2.FragmentCollected(this.gameObject);
    }
}