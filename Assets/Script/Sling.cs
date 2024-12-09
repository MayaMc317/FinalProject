using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sling : MonoBehaviour
{
    public LineRenderer lineRenderer; // Line renderer for the slingshot bands 
    public Transform leftPoint, rightPoint; // The left, right points of the slingshot 
    public Transform projectile; // The projectile to launch
    public Rigidbody projectileRb; // Rigidbody for the projectile 

    public float launchForceMultiplier = 20f; // Multiplier for the launch force
    private bool isDragging = false; // Whether the player is dragging the projectile

    public int lives = 3; // Number of lives for the projectile
    private bool isDepleted = false; // Whether the projectile has been used up

    private Vector3 initialPosition; // The initial position of the projectile

    public TextMeshProUGUI livesText; // TextMeshPro text to display lives
    public TextMeshProUGUI gameOverText; // TextMeshPro text to display game over

    public Button restartButton; // Restart button
    public Button mainMenuButton; // Main Menu button

    private bool slingshotEnabled = false; // Track whether the slingshot is enabled

    void Start()
    {
        lineRenderer.enabled = true; // To make sure the line renderer is initialized 
        lineRenderer.positionCount = 3;

        initialPosition = projectile.position;

        UpdateProjectileState();
        UpdateLivesText();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false); // Hide the game text at the start
            
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false); // Hide the restart button initially
            restartButton.onClick.AddListener(Restart); // Attach the restart function to the button
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(false); // Hide the main menu button initially
            mainMenuButton.onClick.AddListener(Main); // Attach the main menu function to the button
        }

        projectile.position = initialPosition;

    }


    void Update()
    {
        if (lineRenderer.enabled)
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

    /*void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 offset = currentMousePosition - leftPoint.position;
            float maxDistance = 2f; // Maximum stretch distance
            if (offset.magnitude > maxDistance)
            {
                offset = offset.normalized * maxDistance;
            }

            projectile.position = leftPoint.position + offset;
        }
    }*/

    void OnMouseDrag()
    {
        if (isDragging)
        {
        // Get the current mouse position in world space
        Vector3 currentMousePosition = GetMouseWorldPosition();

        // Calculate the direction and distance from the leftPoint to the mouse position
        Vector3 direction = currentMousePosition - leftPoint.position;
        float maxDistance = 5f; // Maximum stretch distance

        // Clamp the distance to ensure the projectile stays within range
        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
        }

        // Update the projectile's position relative to the leftPoint
        projectile.position = leftPoint.position + direction;

        // Update the line renderer to show the slingshot tension
        lineRenderer.SetPosition(1, projectile.position);
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

            StartCoroutine(HandleProjectileLife());
        }
    }

    /*private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return projectile.position;
    }*/ 

    private Vector3 GetMouseWorldPosition()
    {
    Plane plane = new Plane(Vector3.forward, leftPoint.position); // Define a plane at the slingshot level
    Vector2 mousepause = Input.mousePosition;
    Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mousepause.x, mousepause.y, Camera.main.nearClipPlane));
    return point;
    //if (plane.Raycast(point, out float distance))

    //{
        //return ray.GetPoint(distance); // Get the point where the ray intersects the plane
    //}
        return leftPoint.position;
        return rightPoint.position;
    }

    private IEnumerator HandleProjectileLife()
    {
        DecreaseLife();

        // Wait for 3 seconds before resetting
        yield return new WaitForSeconds(3f);

        if (lives > 0 && !isDragging)
        {
            ResetProjectilePosition();
        }
    }

    private void DecreaseLife()
    {
        lives--;
        UpdateLivesText();

        if (lives <= 0)
        {
            isDepleted = true;
            UpdateProjectileState();
            ShowGameOver(); // Show the game over screen when lives reach zero
        }
    }

    private void ResetProjectilePosition()
    {
        projectileRb.isKinematic = true; // Temporarily disable physics
        projectile.position = initialPosition; // Reset the position
        projectileRb.velocity = Vector3.zero; // Stop any motion
        projectileRb.angularVelocity = Vector3.zero; // Stop any rotation
        UpdateProjectileState();
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
            livesText.text = "Lives: " + lives;
        }
    }

     private void ShowGameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true); // Display the Game Over text
            gameOverText.text = "Game Over!"; 
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true); // Show the restart button
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(true); // Show the Main Menu button
        }
    }

    public void Restart()
    {
        // Reset game state
        lives = 3;
        isDepleted = false;

        UpdateLivesText();
        ResetProjectilePosition();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false); // Hide Game Over text
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false); // Hide the restart button
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(false); // Hide the main menu button
        }
    }

    private void Main()
    {
        SceneManager.LoadScene("MainMenu"); // 
    }

    public void SetSlingshotEnabled(bool enabled)
    {
        slingshotEnabled = enabled;
    }

    
}
