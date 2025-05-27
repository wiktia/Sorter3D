using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundries : MonoBehaviour
{

    public Vector3 minBounds = new Vector3(-150, 1, -150);
    public Vector3 maxBounds = new Vector3(150, 10, 150);
    
    void Start()
    {
        
    }

    void Update()
    {
        //aktualna pozycja
        Vector3 currentPosition = transform.position;

        //ograniczenie x
        currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);

        // ograniczenie y
        currentPosition.z = Mathf.Clamp(currentPosition.z, minBounds.z, maxBounds.z);

        //nowa pozycja
        transform.position = currentPosition;
    }
}
