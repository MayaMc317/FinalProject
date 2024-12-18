using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Sling : MonoBehaviour
{
    public LineRenderer lineRenderer; // Line renderer for the slingshot bands 
    public Transform leftPoint, rightPoint; // The left, right points of the slingshot 
    public Transform projectile; // The projectile to launch
    public Rigidbody projectileRb; // Rigidbody for the projectile 

    public float launchForceMultiplier = 12f; // Multiplier for the launch force
    private bool isDragging = false; // Whether the player is dragging the projectile

    public int lives = 3; // Number of lives for the projectile
    private bool isDepleted = false; // Whether the projectile has been used up

    private Vector3 initialPosition; // The initial position of the projectile

    public TextMeshProUGUI livesText; // TextMeshPro text to display lives
    public TextMeshProUGUI gameOverText; // TextMeshPro text to display game over
    public TextMeshProUGUI pauseText; // TextMeshPro text to display pause text
    public TextMeshProUGUI continueText; // TextMeshPro text to display continue text

    public Button restartButton; // Restart button
    public Button mainMenuButton; // Main Menu button
    public Button continueYesButton; // Yes button
    public Button continueNoButton; // No Button

    public VideoPlayer videoPlayer; //Video player component 

    private bool isPause = false; // Tracks if the game is paused 
    private bool slingshotEnabled = false; // Track whether the slingshot is enabled
    private int targetsDestroyed = 0; // Tracks the number of targets destroyed
    private int totalTargets; // total number of targets in the scene

    

    void Start()
    {
        lineRenderer.enabled = true; // To make sure the line renderer is initialized 
        lineRenderer.positionCount = 3; // Sets the number of postions for the linerenderer 

        initialPosition = projectile.position; // Stores the current positon of the projectile in the variable 

        UpdateProjectileState();
        UpdateLivesText();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false); // Hide the game text at the start
            
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false); // Hide the restart button
            restartButton.onClick.AddListener(Restart); // Attach the restart function to the button
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(false); // Hide the main menu button 
            mainMenuButton.onClick.AddListener(Main); // Attach the main menu function to the button
        }

        if (continueText != null)
        {
            continueText.gameObject.SetActive(false); // Hide continue prompt 
        }

        if (continueYesButton != null)
        {
            continueYesButton.gameObject.SetActive(false); // Hide continue prompt 
            continueYesButton.onClick.AddListener(ContinueGame);
        }

        if (continueNoButton != null)
        {
            continueNoButton.gameObject.SetActive(false); // Hide continue prompt 
            continueNoButton.onClick.AddListener(EndGame);
        }


        projectile.position = initialPosition; // Sets the position of the projectile to its initial postion 

        totalTargets = GameObject.FindGameObjectsWithTag("Target").Length; // Calculates total targets in the scen

    }


    void Update()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, leftPoint.position); // Left point
            lineRenderer.SetPosition(1, projectile.position); // Middle point
            lineRenderer.SetPosition(2, rightPoint.position); // Right point
        }

        if(Input.GetKeyDown(KeyCode.Space)) //Game pauses when space is pressed 
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if(isPause)
        {
            Time.timeScale = 1f; // Resume the game
            isPause = false;

            if(pauseText != null)
            pauseText.gameObject.SetActive(false); // Hides pause text

            if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(false); // Hide the main menu button initially
            mainMenuButton.onClick.AddListener(Main); // Attach the main menu function to the button
        }
            if(videoPlayer != null)
            videoPlayer.Play(); // Resume the video
        }
        else
        {
            Time.timeScale = 0f; // Pauses the game
            isPause = true;

            if(pauseText != null)
            pauseText.gameObject.SetActive(true); // "Shows pause text

            if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(true); // Hide the main menu button initially
            mainMenuButton.onClick.AddListener(Main); // Attach the main menu function to the button
        }

            if(videoPlayer != null)
            videoPlayer.Pause(); // Pauses the video
        }
    }

    void OnMouseDown()
    {
        if (!isDepleted && !isPause && Vector3.Distance(GetMouseWorldPosition(), projectile.position) < 1f)
        {
            isDragging = true;
            lineRenderer.enabled = true; // Show the slingshot bands
        }
    }

    void OnMouseDrag()
    {
        if (isDragging && !isPause)
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
        if (isDragging && !isPause)
        {
            isDragging = false;
            lineRenderer.enabled = false; // Hide the slingshot bands

            Vector3 launchDirection = (leftPoint.position - projectile.position).normalized; // Calculates the direction of the lunach 
            float stretchDistance = Vector3.Distance(leftPoint.position, projectile.position); // Calculates the distance between the protectile and left point 

            if (projectileRb != null)
            {
                projectileRb.isKinematic = false; // Ensure physics is active
                projectileRb.velocity = launchDirection * launchForceMultiplier * stretchDistance; // The line sets the velocity off the projectile  rigibody for launching it 
            }

            StartCoroutine(HandleProjectileLife());
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
    Plane plane = new Plane(Vector3.forward, leftPoint.position); // Define a plane at the slingshot level
    Vector2 mousepause = Input.mousePosition; // The line gets the current position of the mouse pointer on the screen 
    Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mousepause.x, mousepause.y, Camera.main.nearClipPlane)); // The line changes the mouses position from screen coordinates to world coordinates 
    return point;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Target"))
        {
            Debug.Log("Ball collided with the target");
            Destroy(other.gameObject); // Destroys the target
            targetsDestroyed++; // Increments the targetDestroyed variable by 1 
            CheckAllTargetDestroyed();
        }
    }

    private void CheckAllTargetDestroyed()
    {
        if(targetsDestroyed >= totalTargets) // Checks if all targets have been destroyed 
        {
            ShowContinuePrompt();
        }
    }

    private void ShowContinuePrompt()
    {
        if (continueText != null)
        {
            continueText.gameObject.SetActive(true);
            continueText.text = "All targets destroyed! Do you want to continue?"; //Shows a text in the game asking the user if they want to continue
        }

        if (continueYesButton != null)
        {
            continueYesButton.gameObject.SetActive(true);
        }

        if (continueNoButton != null)
        {
            continueNoButton.gameObject.SetActive(true);
        }

        Time.timeScale = 0f; // Pause the game while waiting for input
    }

    public void ContinueGame()
    {
        if(continueText != null)
        {
            continueText.gameObject.SetActive(false);
        }

       if(continueYesButton != null)
        {
            continueYesButton.gameObject.SetActive(false);
        }

        if(continueNoButton != null)
        {
            continueNoButton.gameObject.SetActive(false);
        }
        Time.timeScale = 1f; // Resumes the game

        targetsDestroyed = 0; // Resets targetsDestroyed and reload targets
        ReloadTargets();
    }

    private void ReloadTargets()
    {
        Debug.Log("Reloading targets");// Reaoads the targets back in the scene 
    }

    public void EndGame() //Ends the game and goes to the main menu 
    {
        Debug.Log("Ending game");
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator HandleProjectileLife()
    {
        if(!projectileRb.isKinematic) // Checks if the projectile is not kinematic then calls a method to decrease life 
        {
        DecreaseLife();
        }
        // Wait for 3 seconds before resetting
        yield return new WaitForSeconds(3.5f);

        if (lives > -1 && !isDragging) // Checks if the player has remaning lives and if the projectile is not being dragged 
        {
            ResetProjectilePosition();
        }
    }

    private void DecreaseLife()
    {
        lives--; // Decrease the life by 1 and updates the display of the lives count 
        UpdateLivesText();

        if (lives <= -1)
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
        if (livesText != null) // Checks if the livesText UI element is not null and updates its displayed text
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

    public void Restart() // Restarts the game
    {
        lives = 3; // Sets the player lives to 3 
        isDepleted = false; // Depletes the lives 

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
        SceneManager.LoadScene("MainMenu"); //Goes to the main manu 
    }


    public void SetSlingshotEnabled(bool enabled)
    {
        slingshotEnabled = enabled; // Sets the state of the slingshot to either enable or disable 
    }
}
