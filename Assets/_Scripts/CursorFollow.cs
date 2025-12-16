using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    private Camera mainCamera;
    private GameManager gameManager;

    private void Start()
    {
        mainCamera = Camera.main;
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        if (gameManager.currentGameState == GameManager.GameState.Playing)
        {
            transform.position = GetMousePosition();
        }
    }

    /// <summary>
    /// Obtiene la posición del mouse y la convierte a coordenadas del mundo.
    /// </summary>
    /// <returns>La posición del mouse</returns>
    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        return mousePosition;
    }
}
