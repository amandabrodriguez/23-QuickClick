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
    [SerializeField] private GameObject lostFoodPanel;
    [SerializeField] private TextMeshProUGUI gamePausedTMP;
    [SerializeField] private RawImage pauseOrPlayImage;
    [SerializeField] private Image maxScoreImage;
    [SerializeField] private Button pauseOrPlayBtn;
    [SerializeField] private Sprite[] pauseAndPlaySprites; //0: Pause, 1: Play

    public List<GameObject> targetPrefabs;

    [HideInInspector] public GameState currentGameState = GameState.Loading;

    private const string MAX_SCORE_PREFS = "MaxScore";

    private Difficulty currentGameDifficulty = Difficulty.Medium;
    private int maxLostFood = 3;
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
        ShowMaxScore();
    }

    /// <summary>
    /// Spawnea objetivos en la escena a una tasa determinada por la dificultad seleccionada.
    /// </summary>
    /// <param name="difficulty">Dificultad seleccionada al inicio del juego</param>
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
    /// Actualizar la puntuación del jugador y el texto en pantalla según la dificultad.
    /// </summary>
    /// <param name="scoreToAdd">Número de puntos a añadir a la puntuación global.</param>
    public void UpdateScore(int scoreToAdd)
    {
        Score += scoreToAdd + (int)currentGameDifficulty;
        scoreTMP.text = "" + Score;
    }

    /// <summary>
    /// Muestra la puntuación máxima alcanzada en partidas anteriores.
    /// </summary>
    public void ShowMaxScore()
    {
        int maxScore = PlayerPrefs.GetInt(MAX_SCORE_PREFS, 0);
        maxScoreImage.gameObject.SetActive(maxScore > 0);
        scoreTMP.text = maxScore > 0 ? "" + maxScore : "";
    }

    /// <summary>
    /// Actualiza el contador de comidas perdidas y el texto en pantalla; y declara el game over si se pierden más de 2 comidas (good targets).
    /// </summary>
    public void UpdateLostFood()
    {
        if (currentGameState == GameState.Playing)
        {
            foodFailedCount++;
            lostFoodTMP.text = Mathf.Clamp(foodFailedCount, 0, maxLostFood) + "/" + maxLostFood;
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
        SaveMaxScore();
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
        currentGameDifficulty = difficulty;
        maxScoreImage.gameObject.SetActive(true);
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePausedTMP.gameObject.SetActive(false);
        pauseOrPlayBtn.gameObject.SetActive(true);
        lostFoodPanel.SetActive(true);
        pauseOrPlayImage.texture = pauseAndPlaySprites[0].texture; //Pausa
        maxLostFood -= (int)currentGameDifficulty;
        StartCoroutine(SpawnTarget(currentGameDifficulty));
        Score = 0;
        UpdateScore(-(int)currentGameDifficulty);
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
        pauseOrPlayBtn.gameObject.SetActive(false);
        lostFoodPanel.SetActive(false);
    }

    /// <summary>
    /// Guarda la puntuación máxima alcanzada en PlayerPrefs si la puntuación actual es mayor.
    /// </summary>
    private void SaveMaxScore()
    {
        int maxScore = PlayerPrefs.GetInt(MAX_SCORE_PREFS, 0);
        if (score > maxScore)
        {
            PlayerPrefs.SetInt(MAX_SCORE_PREFS, score);
            // TODO: Si hay una nueva puntuación máxima, mostrar algún tipo de notificación al jugador. Partículas.
        }
    }

    /// <summary>
    /// Sale de la aplicación.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
