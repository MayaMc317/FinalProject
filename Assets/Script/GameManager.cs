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

        if (gameUI != null)
        {
            gameUI.SetActive(false); // Hide game elements initially
        }
    }

    void Update()
    {
        // If the game hasn't started, allow starting with any key press
        if (!gameStarted && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true; // Mark the game as started

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

            // Show the main game UI or start the game logic
            if (gameUI != null)
            {
                gameUI.SetActive(true);
            }

            Debug.Log("Game Started!");
        }
    }

    // Method to handle Easy Button click
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
    
}