using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Sling : MonoBehaviour
{
    public LineRenderer lineRenderer; // Line renderer for the slingshot bands 
    public Transform leftPoint, rightPoint; // The left, right points of the slingshot 
    public Transform projectile; // The projectile to launch
    public Rigidbody projectileRb; // Rigidbody for the projectile 

    public float launchForceMultiplier = 10f; // Multiplier for the launch force
    private bool isDragging = false; // Whether the player is dragging the projectile

    public int lives = 3; // Number of lives for the projectile
    private bool isDepleted = false; // Whether the projectile has been used up

    private Vector3 initialPosition; // The initial position of the projectile

    public TextMeshProUGUI livesText; // TextMeshPro text to display lives

    private bool slingshotEnabled = false; // Track whether the slingshot is enabled

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = true; // To make sure the line renderer is initialized 
        lineRenderer.positionCount = 3;

        // Store the initial position of the projectile
        initialPosition = projectile.position;

        UpdateProjectileState();
        UpdateLivesText(); // Update the lives display when the game starts
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
        if (!isDepleted && Vector3.Distance(GetMouseWorldPosition(), projectile.position) < 1f)
        {
            isDragging = true;
            lineRenderer.enabled = true; // Show the slingshot bands
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
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

            Vector3 launchDirection = (leftPoint.position - projectile.position).normalized;
            float stretchDistance = Vector3.Distance(leftPoint.position, projectile.position);

            if (projectileRb != null)
            {
                projectileRb.isKinematic = false; // Ensure physics is active
                projectileRb.velocity = launchDirection * launchForceMultiplier * stretchDistance;
            }

            // Start the process of decreasing life and resetting the projectile
            StartCoroutine(HandleProjectileLife());
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point; // Return the hit point where the mouse ray intersects
        }
        return projectile.position;
    }

    private IEnumerator HandleProjectileLife()
    {
        DecreaseLife(); // Decrease the projectile's life when launched

        // Wait for 5 seconds before resetting
        yield return new WaitForSeconds(5f);

        if (lives > 0 && !isDragging) // Only reset if there are lives left and the projectile is not being dragged
        {
            ResetProjectilePosition();
        }
    }

    private void DecreaseLife()
    {
        lives--;
        UpdateLivesText(); // Update the lives display
        if (lives <= 0)
        {
            isDepleted = true;
            UpdateProjectileState(); // Update the state of the projectile when lives reach zero
        }
    }

    private void ResetProjectilePosition()
    {
        projectileRb.isKinematic = true; // Temporarily disable physics
        projectile.position = initialPosition; // Reset the position
        projectileRb.velocity = Vector3.zero; // Stop any motion
        projectileRb.angularVelocity = Vector3.zero; // Stop any rotation
        UpdateProjectileState(); // Ensure visibility and correct state
    }

    private void UpdateProjectileState()
    {
        if (isDepleted)
        {
            projectileRb.isKinematic = true; // Disable physics
            projectile.gameObject.SetActive(false); // Hide the projectile
        }
        else
        {
            projectileRb.isKinematic = true; // Ensure the projectile is stationary
            projectile.gameObject.SetActive(true); // Make the projectile visible
        }
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives; // Update the text with the current lives
        }
    }

    // enable/disable slingshot
    public void SetSlingshotEnabled(bool enabled)
    {
        slingshotEnabled = enabled;
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        if (!isDragging && lives > 0) // Ensure the projectile can reset
        {
            ResetProjectilePosition();
        }
    }
}
