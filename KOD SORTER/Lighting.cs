using UnityEngine;
using System.Collections;

public class LosoweMiganie : MonoBehaviour
{
    public Light swiatlo;
    public float minimalnyCzas = 0.1f;
    public float maksymalnyCzas = 2f;
    public float maksymalnaIntensywnosc = 1f;
    public float minimalnaIntensywnosc = 0.5f; 
    private float poczatkowaIntensywnosc;

    void Start()
    {
        if (swiatlo == null)
        {
            Debug.LogError("Nie przypisano komponentu Light!");
            return;
        }
        
        poczatkowaIntensywnosc = swiatlo.intensity;
        StartCoroutine(LosoweMiganieCoroutine());
    }

    IEnumerator LosoweMiganieCoroutine()
    {
        while (true)
        {
            
            swiatlo.intensity = maksymalnaIntensywnosc;
            yield return new WaitForSeconds(Random.Range(minimalnyCzas, maksymalnyCzas));

            
            swiatlo.intensity = minimalnaIntensywnosc;
            yield return new WaitForSeconds(Random.Range(minimalnyCzas, maksymalnyCzas));
        }
    }
}
