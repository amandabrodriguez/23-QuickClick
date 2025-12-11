using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO: Implementar diferentes niveles de dificultad que afecten a la velocidad de aparición de los objetivos y a la puntuación obtenida y la cantidad de comida que se pueda perder.
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum GameState
    {
        Loading,
        Playing,
        Paused,
        GameOver,
    }

    public List<GameObject> targetPrefabs;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI lostFoodTMP;
    public TextMeshProUGUI gameOverTMP;

    [HideInInspector] public GameState currentGameState = GameState.Loading;
    [HideInInspector] public bool gameOver = false;

    private readonly float spawnRate = 1.0f;
    private readonly int maxLostFood = 3;
    private int foodFailedCount = -1;
    private int score;

    private int Score
    {
        get { return score; }
        set { score = Mathf.Clamp(value, 0, 99999); } //Evita que la puntuación sea negativa o excesivamente alta
    }

    void Start()
    {
        gameOverTMP.gameObject.SetActive(false);
        currentGameState = GameState.Playing;
        StartCoroutine(SpawnTarget());
        Score = 0;
        UpdateScore(0);
        UpdateLostFood();
    }

    IEnumerator SpawnTarget()
    {
        while (currentGameState == GameState.Playing)
        {
            yield return new WaitForSeconds(spawnRate);
            int idx = Random.Range(0, targetPrefabs.Count);
            Instantiate(targetPrefabs[idx]);
        }
    }

    /// <summary>
    /// Actualizar la puntuación del jugador y el texto en pantalla.
    /// </summary>
    /// <param name="scoreToAdd">Número de puntos a añadir a la puntuación global.</param>
    public void UpdateScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        scoreTMP.text = "Score: " + Score;
    }

    /// <summary>
    /// Actualiza el contador de comidas perdidas y el texto en pantalla; y declara el game over si se pierden más de 2 comidas (good targets).
    /// </summary>
    public void UpdateLostFood()
    {
        if (currentGameState == GameState.Playing)
        {
            foodFailedCount++;
            lostFoodTMP.text = "Lost Food: " + Mathf.Clamp(foodFailedCount, 0, maxLostFood) + "/" + maxLostFood;
        }
        if (foodFailedCount >= maxLostFood)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Declara el fin del juego y muestra el texto de "Game Over".
    /// </summary>
    public void GameOver()
    {
        gameOver = true;
        currentGameState = GameState.GameOver;
        gameOverTMP.gameObject.SetActive(true);
    }

}
