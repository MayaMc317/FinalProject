using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{
    public GameObject titleScreen; // Reference to the Title Screen UI
    public GameObject easyButton; // Reference to the Easy Button
    public GameObject mediumButton; // Reference to the Medium Button
    public GameObject hardButton; // Reference to the Hard Button
    public GameObject extremeButton; // Reference to the Hard Button
    public GameObject gameUI; // Reference to the main game UI or objects to enable when the game starts

    private bool gameStarted = false; // Tracks whether the game has started

    void Start()
    {
        // Initialize the UI states at the start of the game
        if (titleScreen != null)
        {
            titleScreen.SetActive(true); // Show the title screen
        }

        if (easyButton != null)
        {
            easyButton.SetActive(true); // Show the Easy Button
        }

        if (mediumButton != null)
        {
            mediumButton.SetActive(true); // Show the Medium Button
        }

        if (hardButton != null)
        {
            hardButton.SetActive(true); // Show the Hard Button
        }

        if (extremeButton != null)
        {
            extremeButton.SetActive(true); // Show the Extreme Button
        }

        if (gameUI != null)
        {
            gameUI.SetActive(false); // Hide game elements initially
        }
    }

    void Update()
    {
        // If the game hasn't started allow starting with any key press
        if (!gameStarted && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true; //  tells the user that the game has started

            // Hide the Title Screen
            if (titleScreen != null)
            {
                titleScreen.SetActive(false);
            }

            // Hide the Easy Button
            if (easyButton != null)
            {
                //easyButton.SetActive(false);
            }

            // Hide the Medium Button
            if (mediumButton != null)
            {
                //mediumButton.SetActive(false);
            }

            if (hardButton != null)
            {
                
            }

            if (extremeButton != null)
            {
                
            }

            // Show the main game UI
            if (gameUI != null)
            {
                gameUI.SetActive(true);
            }

            Debug.Log("Game Started!");
        }
    }

    // Method to handle Easy Button click and change the scene
    public void OnEasyButtonClicked()
    {
        // Trigger StartGame when the Easy Button is clicked
        StartGame();
        SceneManager.LoadScene("Pingpong");
    }

    // Method to handle Medium Button click and change the scene
    public void OnMediumButtonClicked()
    {
        StartGame();
        //Load a scene Medium when the Medium Button is clicked
        SceneManager.LoadScene("Medium"); 
    }

    // Method to handle Hard Button click and change the scene
    public void OnHardButtonClicked()
    {
        // Trigger StartGame when the hard Button is clicked
        StartGame();
        SceneManager.LoadScene("Hard");
    }

    // Method to handle Extreme Button click and change the scene
    public void OnExtremeButtonClicked()
    {
        // Trigger StartGame when the extreme Button is clicked
        StartGame();
        SceneManager.LoadScene("Extreme");
    }
}