using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Loading,
        Playing,
        Paused,
        GameOver
    }

    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI lostFoodTMP;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private TextMeshProUGUI gamePausedTMP;
    [SerializeField] private RawImage pauseOrPlayImage;
    [SerializeField] private Button pauseOrPlayBtn;
    [SerializeField] private GameObject gameStatisticsPanel;
    [SerializeField] private Sprite[] pauseAndPlaySprites; //0: Pause, 1: Play

    public List<GameObject> targetPrefabs;


    [HideInInspector] public GameState currentGameState = GameState.Loading;

    private readonly int maxLostFood = 3;
    private float spawnRate = 1.0f;
    private int foodFailedCount = -1;
    private int score;

    private int Score
    {
        get { return score; }
        set { score = Mathf.Clamp(value, 0, 99999); } //Evita que la puntuación sea negativa o excesivamente alta
    }

    void Start()
    {
        StartConfiguration();
    }

    IEnumerator SpawnTarget(Difficulty difficulty)
    {
        spawnRate /= (float)difficulty + 1;
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
        currentGameState = GameState.GameOver;
        gameOverPanel.SetActive(true);
    }


    /// <summary>
    /// Pausa o reanuda el juego según el estado actual (Playing o Paused).
    /// </summary>
    public void PauseGame()
    {
        if (currentGameState == GameState.Playing)
        {
            currentGameState = GameState.Paused;
            pauseOrPlayImage.texture = pauseAndPlaySprites[1].texture; //Play
            Time.timeScale = 0f;
        }
        else if (currentGameState == GameState.Paused)
        {
            currentGameState = GameState.Playing;
            pauseOrPlayImage.texture = pauseAndPlaySprites[0].texture; //Pausa
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Método que inicia la partida desde el menú principal.
    /// </summary>
    /// <param name="difficulty">Número entero que indica la dificultad del juego</param>
    public void StartGame(Difficulty difficulty)
    {
        currentGameState = GameState.Playing;
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePausedTMP.gameObject.SetActive(false);
        gameStatisticsPanel.SetActive(true);
        pauseOrPlayBtn.gameObject.SetActive(true);
        pauseOrPlayImage.texture = pauseAndPlaySprites[0].texture; //Pausa
        StartCoroutine(SpawnTarget(difficulty));
        Score = 0;
        UpdateScore(0);
        UpdateLostFood();
    }

    /// <summary>
    /// Reiniciar el juego recargando la escena actual.
    /// </summary>
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Configuración inicial del juego al arrancar la aplicación.
    /// </summary>
    void StartConfiguration()
    {
        currentGameState = GameState.Loading;
        mainMenuPanel.SetActive(true);
        gameStatisticsPanel.SetActive(false);
        pauseOrPlayBtn.gameObject.SetActive(false);
    }
}
