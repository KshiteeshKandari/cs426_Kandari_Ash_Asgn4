using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int score;
    public TMP_Text scoreText; // Assign in inspector
    public TMP_Text winText; // Assign in inspector

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        score = 0;
        winText.gameObject.SetActive(false);
    }

    public void AddScore(int increment)
    {
        score += increment;
        scoreText.text = "Score: " + score;
        if (score >= 5)
        {
            winText.gameObject.SetActive(true);
            winText.text = "You won the game!";
        }
    }
}
