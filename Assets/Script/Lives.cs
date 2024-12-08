using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lives : MonoBehaviour
{
    public TextMeshProGUI livesText;
    private int Balllives;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame(int start)
    {
        spawnRate /= start;

        isGameActive = true;
        StartCoroutine(SpawnTarget());
        lives = 0;
        UpdateLives(0);
        UpdateLives(3);
    }
}

    /*public void UpdateLives(int lives)
    {
    Balllives += lives;
    livesText.text = "Lives: " + lives;
    if(lives <= 0)
    {
        GameOver();
        titleScreen.gameObject.SetActive(false);
    } */


 