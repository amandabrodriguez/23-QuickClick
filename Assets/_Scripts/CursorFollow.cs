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

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, mousePosition.z + 1);
        return mousePosition;
    }
}
