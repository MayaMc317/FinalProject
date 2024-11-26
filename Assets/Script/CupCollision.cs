using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cup")) //Checks if the object collides with the cup
        {
            Destroy(other.gameObject); // Destroys the cup

            Destroy(gameObject); // Destroys the ball
        }
    }
}
