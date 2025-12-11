using UnityEngine;

public class Target : MonoBehaviour
{

    [Range(-100, 100)]
    public int pointValue;
    public ParticleSystem explosionParticle;

    private Rigidbody _rigidbody;
    private GameManager gameManager;

    private float minForce = 16f;
    private float maxForce = 18f;
    private float torqueForce = 2f;
    private float xRange = 4f;
    private float ySpawnPos = -6f;
    private int penalizationPoints = -10;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();
        _rigidbody.AddForce(RandomForce(), ForceMode.Impulse);
        _rigidbody.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPosition();
    }

    private void OnMouseDown()
    {
        if (gameManager.currentGameState == GameManager.GameState.Playing)
        {
            //Ver como configurar el proyecto para que el nuevo Input System funcione
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            if (gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.UpdateScore(pointValue);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KillZone"))
        {
            if (gameObject.CompareTag("Good"))
            {
                gameManager.UpdateLostFood();
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Genera una fuerza aleatoria hacia arriba dentro de un rango especificado.
    /// </summary>
    /// <returns>Fuerza aleatoria para arriba</returns>
    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minForce, maxForce);
    }

    /// <summary>
    /// Genera un torque aleatorio dentro de un rango especificado.
    /// </summary>
    /// <returns>Valor aleatorio entre -torqueForce y torqueForce</returns>
    private float RandomTorque()
    {
        return Random.Range(-torqueForce, torqueForce);
    }

    /// <summary>
    /// Genera una posición de spawn aleatoria dentro de un rango especificado en el eje X.
    /// </summary>
    /// <returns>Posición aleatoria en 3D, con la coordenada z =0 </returns>
    private Vector3 RandomSpawnPosition()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
