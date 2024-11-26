using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class Sling : MonoBehaviour
{
    
    public LineRenderer lineRenderer; // Line renderer for the slingshot bands 
    public Transform leftPoint, rightPoint; // The left, right points of the slingshot 
    public Transform projectile; // The projectile to launch
    public Rigidbody projectileRb; // Rigidbody for the projectile 
    
    public float launchForceMultiplier = 10f; // Multiplier for the launch force
    private Vector3 dragStartPoint; // The starting point of the drag
    private bool isDragging = false; // Whether the player is dragging the projectile



    // Start is called before the first frame update
    void Start()
    {
       lineRenderer.enabled = true; // To make sure the line renderer is initialized 
       lineRenderer.positionCount = 3;
    }

    // Update is called once per frame
    void Update()
    {
    if (lineRenderer.enabled) // Updating the slingshot bands when the linerenderer is active 
    {

        lineRenderer.SetPosition(0, leftPoint.position); // Left point
        lineRenderer.SetPosition(1, projectile.position); // Middle point
        lineRenderer.SetPosition(2, rightPoint.position); // Right point 
    }
    }
    
    void OnMouseDown()
    {
        // Check if the mouse click is near the projectile
        if (Vector3.Distance(GetMouseWorldPosition(), projectile.position) < 1f)
        {
            isDragging = true;
            lineRenderer.enabled = true; // Show the slingshot bands
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // Move the projectile to follow the mouse position, constrained to a certain range
            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 offset = currentMousePosition - leftPoint.position;
            float maxDistance = 3f; // Maximum stretch distance
            if (offset.magnitude > maxDistance)
            {
                offset = offset.normalized * maxDistance;
            }

            projectile.position = leftPoint.position + offset;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
           isDragging = false;
           lineRenderer.enabled = false; // Hide the slingshot bands

            // Calculate the launch direction and force
            Vector3 launchDirection = (leftPoint.position - projectile.position).normalized;
            float stretchDistance = Vector3.Distance(leftPoint.position, projectile.position);

            // Apply the launch force to the Rigidbody
            if (projectileRb != null)
            {
                projectileRb.isKinematic = false; // Ensure physics is active
                projectileRb.velocity = launchDirection * launchForceMultiplier * stretchDistance;
            }

            
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in world space using a raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point; // Return the hit point where the mouse ray intersects
        }

        // If no collider is hit, return a default position
        return projectile.position;
    }
}
