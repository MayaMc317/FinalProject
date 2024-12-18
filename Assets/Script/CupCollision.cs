using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupCollision : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ball"))
        {
            Destroy(gameObject); // Destroys the ball
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Target")) //Checks if the object collides with the cup
        {
            Destroy(other.gameObject);
            Debug.Log("Collision detected with: " + other.tag);
        }

        if(other.CompareTag("Cup")) //Checks if the object collides with the cup
        {
            Destroy(other.gameObject);
            Debug.Log("Collision detected with: " + other.tag);
        }

    }
   
}
