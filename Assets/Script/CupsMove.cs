using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsMove : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed of movement
    public float moveRange = 0.5f; // Range of movement

    private Vector3 startPosition; // To store the initial position

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // Save the starting position
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new position using a sine wave for smooth back-and-forth motion
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveRange;
        transform.position = startPosition + new Vector3(offset, 0, 0); // Move along the X-axis
    }
}
