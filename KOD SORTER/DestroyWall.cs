using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public Rigidbody rb;
    public Shape.ShapeType requiredShape;
    public bool isDestroyed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDestroyed)
        {
            Shape shape = collision.gameObject.GetComponent<Shape>();
            if (shape != null && shape.shapeType == requiredShape)
            {
                rb.isKinematic = false; 
                isDestroyed = true;

                // zwiększ licznik zniszczonych ścian w PlayerController2
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) 
                {
                    PlayerController2 playerController2 = player.GetComponent<PlayerController2>(); 
                    if (playerController2 != null) 
                    {
                        playerController2.IncrementDestroyedWalls(); 
                    }
                }
            }
        }
    }
}