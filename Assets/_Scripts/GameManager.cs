using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targetPrefabs;
    public bool gameOver = false;
    public TextMeshProUGUI scoreTMP;

    private readonly float spawnRate = 1.0f;
    private int score;

    private int Score
    {
        get { return score; }
        set { score = Mathf.Clamp(value, 0, 99999); }
    }

    void Start()
    {
        StartCoroutine(SpawnTarget());
        Score = 0;
        UpdateScore(0);
    }

    IEnumerator SpawnTarget()
    {
        while (!gameOver)
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

}
