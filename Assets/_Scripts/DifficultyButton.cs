using UnityEngine;
using UnityEngine.UI;

// TODO: Implementar diferentes niveles de dificultad que afecten a la velocidad de aparición de los objetivos y a la puntuación obtenida y la cantidad de comida que se pueda perder.
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class DifficultyButton : MonoBehaviour
{
    public Difficulty difficultyLevel;

    private Button _button;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetDifficulty);
    }

    /// <summary>
    /// Modifica la dificultad del juego según el botón presionado.
    /// </summary>
    public void SetDifficulty()
    {
        gameManager.StartGame(difficultyLevel);
    }
}
